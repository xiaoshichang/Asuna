
namespace AsunaServer.Core
{
    public delegate void OnAcceptHandler(IntPtr session);
    public delegate void OnDisconnectHandler(IntPtr session);
    public delegate void OnReceivePackageHandler(IntPtr session, byte[] data, int length);
    
    
    public static partial class Interface
    {
        public static extern void TcpNetwork_SetAcceptHandler(OnAcceptHandler handler);
        public static extern void TcpNetwork_SetReceiveHandler(OnReceivePackageHandler handler);
        public static extern void TcpNetwork_SetDisconnectHandler(OnDisconnectHandler handler);
        public static extern void TcpNetwork_Send();
    }
}