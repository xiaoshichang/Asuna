using System.Runtime.InteropServices;
using AsunaServer.Core;
using AsunaServer.Foundation.Log;

namespace AsunaServer.Foundation.Network
{
    public delegate void OnAcceptCallback(TcpSession session);
    public delegate void OnConnectCallback(TcpSession session);
    
    public static class InnerNetwork
    {
        public static void Init(string ip, int port,OnAcceptCallback onAccept)
        {
            _onAcceptCallback = onAccept;
            Interface.InnerNetwork_Init(ip, port, OnAccept, OnDisconnect);
        }
        
        private static void OnAccept(IntPtr connection)
        {
            if (_Sessions.ContainsKey(connection))
            {
                Logger.Error($"session({connection}) already exist.");
                return;
            }
            
            var session = new TcpSession(connection, true);
            _Sessions.Add(connection, session);
            _onAcceptCallback?.Invoke(session);
        }

        private static void OnDisconnect(IntPtr connection)
        {
            if (_Sessions.TryGetValue(connection, out var session))
            {
                _Sessions.Remove(connection);
            }
            else
            {
                Logger.Warning("session not exist.");
            }
        }

        public static void ConnectTo(string ip, int port, OnConnectCallback callback)
        {
            _OnConnectCallback = callback;
            Interface.InnerNetwork_ConnectTo(ip, port, OnConnect);
        }

        private static void OnConnect(IntPtr connection)
        {
            var session = new TcpSession(connection, true);
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
        private static OnAcceptCallback? _onAcceptCallback;
        
        /// <summary>
        /// onConnect上层业务回调
        /// </summary>
        private static OnConnectCallback? _OnConnectCallback;
        
        /// <summary>
        /// 维护所有有效链接
        /// </summary>
        private static readonly Dictionary<IntPtr, TcpSession> _Sessions = new();

    }
}


