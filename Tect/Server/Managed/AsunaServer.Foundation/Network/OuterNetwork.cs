using AsunaServer.Core;
using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Network.Message.Serializer;

namespace AsunaServer.Foundation.Network
{
    public static class OuterNetwork
    {
        public static void Init(string ip, int port, OnAcceptCallback? onAccept, OnConnectCallback? onConnect, OnReceiveCallback? onReceive)
        {
            _onAcceptCallback = onAccept;
            _OnConnectCallback = onConnect;
            _onReceiveCallback = onReceive;
            Interface.OuterNetwork_Init(ip, port, OnAccept, OnDisconnect);
        }
        
        private static void OnReceiveMessage(IntPtr connection, object message, Type type)
        {
            if (_Sessions.TryGetValue(connection, out var session))
            {
                _onReceiveCallback?.Invoke(session, message, type);
                return;
            }
            Logger.Warning("OnReceiveMessage connection does not exist!");
        }
        
        private static void OnAccept(IntPtr connection)
        {
            Logger.Debug($"Outer Network OnAccept {connection}");
            if (_Sessions.ContainsKey(connection))
            {
                Logger.Error($"session({connection}) already exist.");
                return;
            }
            
            var session = new TcpSession(connection, true, OnReceiveMessage);
            _Sessions.Add(connection, session);
            _onAcceptCallback?.Invoke(session);
        }

        private static void OnDisconnect(IntPtr connection)
        {
            Logger.Debug($"Outer Network OnDisconnect {connection}");
            if (_Sessions.TryGetValue(connection, out var session))
            {
                session.OnDisconnect();
                _Sessions.Remove(connection);
            }
            else
            {
                Logger.Warning("session not exist.");
            }
        }

        /// <summary>
        /// onAccept上层业务回调
        /// </summary>
        private static OnAcceptCallback? _onAcceptCallback;

        /// <summary>
        /// onReceive上层业务回调
        /// </summary>
        private static OnReceiveCallback? _onReceiveCallback;

        /// <summary>
        /// onConnect上层业务回调
        /// </summary>
        private static OnConnectCallback? _OnConnectCallback;
        
        /// <summary>
        /// 维护所有有效链接
        /// </summary>
        private static readonly Dictionary<IntPtr, TcpSession> _Sessions = new();

        /// <summary>
        /// 使用的序列化器
        /// </summary>
        public static ISerializer Serializer = new JsonSerializer();
    }
}

