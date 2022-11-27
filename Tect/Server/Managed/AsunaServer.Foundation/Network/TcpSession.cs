using System.Runtime.InteropServices;
using AsunaServer.Core;
using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Network.Message.Indexer;
using AsunaServer.Foundation.Network.Message.Serializer;

namespace AsunaServer.Foundation.Network
{
    public delegate void ReceiveMessageHandler(IntPtr connection, object message, Type type);
    
    
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

        public void OnReceive(IntPtr connection, IntPtr rawData, int length, uint index)
        {
            Marshal.Copy(rawData, _ReceiveBuffer, 0, length);
            Type type = AssemblyRegisterIndexer.Instance.GetType(index);
            var message = JsonSerializer.Instance.Deserialize(_ReceiveBuffer, length, type);
            _OnReceiveHandler?.Invoke(connection, message, type);
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
        
        public void Send(INetworkMessage message)
        {
            if (Interface.Connection_IsSending(_Connection))
            {
                _SendQueue.Enqueue(message);
                return;
            }
            var data = JsonSerializer.Instance.Serialize(message);
            var index = AssemblyRegisterIndexer.Instance.GetIndex(message.GetType());
            Marshal.Copy(data, 0, _SendBuffer, data.Length);
            if (_InnerNetwork)
            {
                Interface.InnerNetwork_Send(_Connection, _SendBuffer, data.Length, index);
            }
            else
            {
                Interface.OuterNetwork_Send(_Connection, _SendBuffer, data.Length, index);
            }
        }
        
        private readonly IntPtr _Connection;
        private readonly IntPtr _SendBuffer;
        private readonly Queue<INetworkMessage> _SendQueue = new();

        private readonly byte[] _ReceiveBuffer;
        private readonly bool _InnerNetwork;

        private ReceiveMessageHandler _OnReceiveHandler;
        
        private const int RECEIVE_BUFFER_SIZE = 2048;
        private const int SEND_BUFFER_SIZE = 2048;
    }
}

