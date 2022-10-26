using AsunaServer.Core;
using AsunaServer.Foundation.Log;

namespace AsunaServer.Foundation.Network
{
    public static class OuterNetwork
    {
        public static void Init()
        {
            Interface.OuterNetwork_Init();
            Interface.OuterNetwork_SetAcceptHandler(OnAccept);
            Interface.OuterNetwork_SetReceiveHandler(OnReceivePackage);
            Interface.OuterNetwork_SetDisconnectHandler(OnDisconnect);
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

