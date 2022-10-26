
namespace AsunaServer.Core
{
    public delegate void OnAcceptHandler(IntPtr session);
    public delegate void OnDisconnectHandler(IntPtr session);
    public delegate void OnReceivePackageHandler(IntPtr session, byte[] data, int length);
    
    
    public static partial class Interface
    {
        public static extern void InnerNetwork_Init();
        public static extern void InnerNetwork_SetAcceptHandler(OnAcceptHandler handler);
        public static extern void InnerNetwork_SetReceiveHandler(OnReceivePackageHandler handler);
        public static extern void InnerNetwork_SetDisconnectHandler(OnDisconnectHandler handler);
        public static extern void InnerNetwork_Send();
        
        public static extern void OuterNetwork_Init();
        public static extern void OuterNetwork_SetAcceptHandler(OnAcceptHandler handler);
        public static extern void OuterNetwork_SetReceiveHandler(OnReceivePackageHandler handler);
        public static extern void OuterNetwork_SetDisconnectHandler(OnDisconnectHandler handler);
        public static extern void OuterNetwork_Send();
    }
}