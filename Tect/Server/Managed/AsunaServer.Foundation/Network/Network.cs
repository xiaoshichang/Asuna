using AsunaServer.Core;
using AsunaServer.Foundation.Logger;

namespace XServer.Foundation.Network
{
    public static class Network
    {
        public static void Init()
        {
            Interface.TcpNetwork_SetAcceptHandler(OnAccept);
            Interface.TcpNetwork_SetDisconnectHandler(OnDisconnect);
            Interface.TcpNetwork_SetReceiveHandler(OnReceivePackage);
        }
        
        private static void OnAccept(IntPtr session)
        {
            
        }

        private static void OnDisconnect(IntPtr session)
        {
            
        }

        private static void OnReceivePackage(IntPtr connection, byte[] data, int length)
        {
            if (_Sessions.TryGetValue(connection, out var session))
            {
                session.OnReceive(data, length);
            }
            else
            {
                Logger.Warning("session not exist!");
            }
        }

        private static readonly Dictionary<IntPtr, TcpSession> _Sessions = new();

    }
}


