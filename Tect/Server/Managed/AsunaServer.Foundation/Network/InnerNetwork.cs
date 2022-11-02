using System.Runtime.InteropServices;
using AsunaServer.Core;
using AsunaServer.Foundation.Log;

namespace AsunaServer.Foundation.Network
{
    public static class InnerNetwork
    {
        public static void Init(string ip, int port)
        {
            Interface.InnerNetwork_Init(ip, port, OnAccept, OnReceive, OnDisconnect);
        }
        
        private static void OnAccept(IntPtr connection)
        {
            if (_Sessions.ContainsKey(connection))
            {
                Logger.Error($"session({connection}) already exist.");
                return;
            }
            
            var session = new TcpSession(connection, Send);
            _Sessions.Add(connection, session);
            session.OnAccept();
        }

        private static void OnDisconnect(IntPtr connection)
        {
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

        private static void OnReceive(IntPtr connection, IntPtr data, uint length, uint type)
        {
            if (_Sessions.TryGetValue(connection, out var session))
            {
                session.OnReceive(data, length, type);
            }
        }

        public static void Send(IntPtr connection, IntPtr data, uint type)
        {
            Interface.InnerNetwork_Send(connection, data, type);
        }

        private static readonly Dictionary<IntPtr, TcpSession> _Sessions = new();

    }
}


