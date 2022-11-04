using System.Runtime.InteropServices;
using AsunaServer.Core;
using AsunaServer.Foundation.Log;

namespace AsunaServer.Foundation.Network
{
    public static class InnerNetwork
    {
        public static void Init(string ip, int port)
        {
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
        
        private static readonly Dictionary<IntPtr, TcpSession> _Sessions = new();

    }
}


