using AsunaServer.Core;
using AsunaServer.Foundation.Log;

namespace AsunaServer.Foundation.Network
{
    public static class OuterNetwork
    {
        public static void Init()
        {
        }
        
        private static void OnAccept(IntPtr session)
        {
            
        }

        private static void OnDisconnect(IntPtr session)
        {
            
        }

        private static void OnReceivePackage(IntPtr connection, IntPtr data, uint length, uint type)
        {
           
        }

        private static readonly Dictionary<IntPtr, TcpSession> _Sessions = new();
    }
}

