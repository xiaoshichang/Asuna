using AsunaServer.Core;
using AsunaServer.Foundation.Log;

namespace AsunaServer.Foundation.Network
{
    public static class InnerNetwork
    {
        public static void Init()
        {
            Interface.InnerNetwork_Init();
            Interface.InnerNetwork_SetAcceptHandler(OnAccept);
            Interface.InnerNetwork_SetReceiveHandler(OnReceivePackage);
            Interface.InnerNetwork_SetDisconnectHandler(OnDisconnect);
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


