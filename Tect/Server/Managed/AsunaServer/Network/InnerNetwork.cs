
using AsunaServer.Core;
using AsunaServer.Debug;
using AsunaServer.Message.Serializer;


namespace AsunaServer.Network
{
    public delegate void OnAcceptCallback(TcpSession session);
    public delegate void OnConnectCallback(TcpSession session);
    public delegate void OnReceiveCallback(TcpSession session, object message);
    public delegate void OnDisconnectCallback(TcpSession session);
    
    public static class InnerNetwork
    {
        public static void Init(string ip, int port, 
            OnAcceptCallback? onAccept, 
            OnConnectCallback? onConnect, 
            OnReceiveCallback? onReceive,
            OnDisconnectCallback onDisconnect)
        {
            _OnAcceptCallback = onAccept;
            _OnConnectCallback = onConnect;
            _OnReceiveCallback = onReceive;
            _OnDisconnectCallback = onDisconnect;
            Interface.InnerNetwork_Init(ip, port, _OnAccept, _OnDisconnect);
        }
        
        private static void _OnAccept(IntPtr connection)
        {
            Logger.Debug($"Inner Network OnAccept {connection}");
            if (_Sessions.ContainsKey(connection))
            {
                Logger.Error($"session({connection}) already exist.");
                return;
            }
            
            var session = new TcpSession(connection, true, _OnReceiveMessage);
            _Sessions.Add(connection, session);
            _OnAcceptCallback?.Invoke(session);
        }

        private static void _OnReceiveMessage(IntPtr connection, object message)
        {
            if (_Sessions.TryGetValue(connection, out var session))
            {
                _OnReceiveCallback?.Invoke(session, message);
                return;
            }
            Logger.Warning("OnReceiveMessage connection does not exist!");
        }
        
        /// <summary>
        /// 来自core的回调 - 链接断开
        /// </summary>
        private static void _OnDisconnect(IntPtr connection)
        {
            Logger.Debug($"Inner Network OnDisconnect {connection}");
            if (_Sessions.TryGetValue(connection, out var session))
            {
                _OnDisconnectCallback?.Invoke(session);
                session.OnDisconnect();
                _Sessions.Remove(connection);
            }
            else
            {
                Logger.Warning("session not exist.");
            }
        }

        public static void ConnectTo(string ip, int port)
        {
            Logger.Info($"connect to {ip} {port}");
            Interface.InnerNetwork_ConnectTo(ip, port, _OnConnect);
        }

        private static void _OnConnect(IntPtr connection)
        {
            var session = new TcpSession(connection, true, _OnReceiveMessage);
            _Sessions.Add(connection, session);
            _OnConnectCallback?.Invoke(session);
        }

        public static int GetConnectionCount()
        {
            return _Sessions.Count;
        }

        /// <summary>
        /// onAccept上层业务回调
        /// </summary>
        private static OnAcceptCallback? _OnAcceptCallback;

        /// <summary>
        /// onReceive上层业务回调
        /// </summary>
        private static OnReceiveCallback? _OnReceiveCallback;

        /// <summary>
        /// onConnect上层业务回调
        /// </summary>
        private static OnConnectCallback? _OnConnectCallback;
        
        /// <summary>
        /// onDisconnect 上层业务回调
        /// </summary>
        private static OnDisconnectCallback? _OnDisconnectCallback;
        
        /// <summary>
        /// 维护所有有效链接
        /// </summary>
        private static readonly Dictionary<IntPtr, TcpSession> _Sessions = new();
        
        /// <summary>
        /// 使用的序列化器
        /// </summary>
        public static SerializerBase Serializer = new ProtobufSerializer();

    }
}


