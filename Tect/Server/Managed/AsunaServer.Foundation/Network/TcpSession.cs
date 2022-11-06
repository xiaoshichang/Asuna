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
            _SendBuffer = Marshal.AllocHGlobal(SEND_BUFFER_SIZE);
            _ReceiveBuffer = new byte[RECEIVE_BUFFER_SIZE];
            
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
            Marshal.Copy(data, 0, _SendBuffer, data.Length);
            if (_InnerNetwork)
            {
                Interface.InnerNetwork_Send(_Connection, _SendBuffer, (uint)data.Length, type);
            }
            else
            {
                Interface.OuterNetwork_Send(_Connection, _SendBuffer, (uint)data.Length, type);
            }
        }
        
        private readonly IntPtr _Connection;
        private readonly IntPtr _SendBuffer;
        private readonly byte[] _ReceiveBuffer;
        private readonly bool _InnerNetwork;

        private const int RECEIVE_BUFFER_SIZE = 2048;
        private const int SEND_BUFFER_SIZE = 2048;
    }
}

