using System.Runtime.InteropServices;
using AsunaServer.Core;

namespace AsunaServer.Foundation.Network
{
    public delegate void SendDataDelegate(IntPtr connection, IntPtr data,uint length, uint type);
    
    public class TcpSession
    {
        public TcpSession(IntPtr connection, bool innerNetwork)
        {
            _Connection = connection;
            _InnerNetwork = innerNetwork;
            
            Interface.Connection_SetReceiveCallback(_Connection, OnReceive);
            Interface.Connection_SetSendCallback(_Connection, OnSend);
        }

        public void OnReceive(IntPtr data, uint length, uint type)
        {
        }

        public void OnSend()
        {
        }
        
        public void Send(byte[] data, uint type)
        {
            IntPtr retVal = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, retVal, data.Length);
            if (_InnerNetwork)
            {
                Interface.InnerNetwork_Send(_Connection, retVal, (uint)data.Length, type);
            }
            else
            {
                Interface.OuterNetwork_Send(_Connection, retVal, (uint)data.Length, type);
            }
        }
        
        private readonly IntPtr _Connection;
        private readonly bool _InnerNetwork;
    }
}

