
using System.Runtime.InteropServices;

namespace AsunaServer.Core
{
    public delegate void OnAcceptHandler(IntPtr session);
    public delegate void OnDisconnectHandler(IntPtr session);
    public delegate void OnReceivePackageHandler(IntPtr session, IntPtr data, uint length, uint type);
    
    
    public static partial class Interface
    {
        [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
        public static extern void InnerNetwork_Init(
            string ip, 
            int port, 
            OnAcceptHandler onAccept,
            OnReceivePackageHandler onReceive,
            OnDisconnectHandler onDisconnect);
        
        [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
        public static extern void InnerNetwork_Send(IntPtr session, IntPtr data, uint length);
        
        [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
        public static extern void OuterNetwork_Init(
            string ip, 
            int port, 
            OnAcceptHandler onAccept,
            OnReceivePackageHandler onReceive,
            OnDisconnectHandler onDisconnect);
        
        [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
        public static extern void OuterNetwork_Send(IntPtr session, IntPtr data, uint length);
    }
}