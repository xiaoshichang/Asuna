using System.Runtime.InteropServices;

namespace AsunaServer.Foundation.Network
{
    public delegate void SendDataDelegate(IntPtr connection, IntPtr data, uint type);
    
    public class TcpSession
    {
        public TcpSession(IntPtr connection, SendDataDelegate sendDataDelegate)
        {
            _Connection = connection;
            _SendDataDelegate = sendDataDelegate;
        }

        public void OnAccept()
        {
            
        }

        public void OnReceive(IntPtr data, uint length, uint type)
        {
            
        }

        public void OnDisconnect()
        {
            
        }

        public void Send(byte[] data, uint type)
        {
            IntPtr retVal = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, retVal, data.Length);
            _SendDataDelegate.Invoke(_Connection, retVal, type);
        }
        
        private readonly IntPtr _Connection;
        private readonly SendDataDelegate _SendDataDelegate;
    }
}

