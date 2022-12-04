using System.Runtime.InteropServices;
using AsunaServer.Core;

namespace AsunaServer.Foundation.Network
{
    public delegate void ReceiveMessageHandler(IntPtr connection, object message);
    
    
    public class TcpSession
    {
        public TcpSession(IntPtr connection, bool innerNetwork, ReceiveMessageHandler onReceive)
        {
            _Connection = connection;
            _InnerNetwork = innerNetwork;
            _SendBuffer = Marshal.AllocHGlobal(SEND_BUFFER_SIZE);
            _ReceiveBuffer = new byte[RECEIVE_BUFFER_SIZE];
            _OnReceiveHandler = onReceive;
            
            Interface.Connection_SetReceiveCallback(_Connection, OnReceive);
            Interface.Connection_SetSendCallback(_Connection, OnSend);
        }

        public IntPtr GetConnectionID()
        {
            return _Connection;
        }

        public void OnReceive(IntPtr connection, IntPtr rawData, int length, uint index)
        {
            Marshal.Copy(rawData, _ReceiveBuffer, 0, length);
            if (_InnerNetwork)
            {
                var message = InnerNetwork.Serializer.Deserialize(_ReceiveBuffer, length, index);
                _OnReceiveHandler?.Invoke(connection, message);

            }
            else
            {
                var message = OuterNetwork.Serializer.Deserialize(_ReceiveBuffer, length, index);
                _OnReceiveHandler?.Invoke(connection, message);
            }
        }

        public void OnSend()
        {
            if (_SendQueue.Count <= 0)
            {
                return;
            }

            var message = _SendQueue.Dequeue();
            Send(message);
        }
        
        public void Send(object message)
        {
            if (Interface.Connection_IsSending(_Connection))
            {
                _SendQueue.Enqueue(message);
                return;
            }
            
            if (_InnerNetwork)
            {
                var data = InnerNetwork.Serializer.Serialize(message);
                Marshal.Copy(data, 0, _SendBuffer, data.Length);
                var index = InnerNetwork.Serializer.GetIndexByType(message.GetType());
                Interface.InnerNetwork_Send(_Connection, _SendBuffer, data.Length, index);
            }
            else
            {
                var data = OuterNetwork.Serializer.Serialize(message);
                Marshal.Copy(data, 0, _SendBuffer, data.Length);
                var index = OuterNetwork.Serializer.GetIndexByType(message.GetType());
                Interface.OuterNetwork_Send(_Connection, _SendBuffer, data.Length, index);
            }
        }

        public void OnDisconnect()
        {
            
        }
        
        private readonly IntPtr _Connection;
        private readonly IntPtr _SendBuffer;
        private readonly Queue<object> _SendQueue = new();

        private readonly byte[] _ReceiveBuffer;
        private readonly bool _InnerNetwork;

        private readonly ReceiveMessageHandler _OnReceiveHandler;
        
        private const int RECEIVE_BUFFER_SIZE = 2048;
        private const int SEND_BUFFER_SIZE = 2048;
    }
}

