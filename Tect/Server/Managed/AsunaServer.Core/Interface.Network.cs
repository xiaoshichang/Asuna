
namespace AsunaServer.Core
{
    
    
    public static partial class Interface
    {
        public static extern void TcpNetwork_SetAcceptHandler();
        public static extern void TcpNetwork_SetReceiveHandler();
        public static extern void TcpNetwork_SetDisconnect();
        public static extern void TcpNetwork_Send();
    }
}