
using System.Runtime.InteropServices;

namespace AsunaServer.Core
{
    public delegate void OnAcceptHandler(IntPtr session);
    public delegate void OnConnectHandler(IntPtr session);
    public delegate void OnDisconnectHandler(IntPtr session);
    public delegate void OnReceivePackageHandler(IntPtr session, IntPtr data, int length, uint type);
    public delegate void OnSendHandler();
    
    
    public static partial class Interface
    {
        [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
        public static extern void InnerNetwork_Init(
            string ip, 
            int port, 
            OnAcceptHandler onAccept,
            OnDisconnectHandler onDisconnect);

        [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
        public static extern void InnerNetwork_ConnectTo(string ip, int port, OnConnectHandler callback);
        
        [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
        public static extern void InnerNetwork_Send(IntPtr session, IntPtr data, int length, uint type);
        
        [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
        public static extern void OuterNetwork_Init(
            string ip, 
            int port, 
            OnAcceptHandler onAccept,
            OnDisconnectHandler onDisconnect);
        
        [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
        public static extern void OuterNetwork_Send(IntPtr session, IntPtr data, int length, uint type);
        
        [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
        public static extern bool Connection_IsSending(IntPtr session);

        [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
        public static extern void Connection_SetReceiveCallback(IntPtr session, OnReceivePackageHandler onReceiveCallback);

        [DllImport(_DllPath, CallingConvention = _CallingConvention, CharSet = _CharSet)]
        public static extern void Connection_SetSendCallback(IntPtr session, OnSendHandler onReceiveCallback);
    }
}