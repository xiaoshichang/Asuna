using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using AsunaClient.Foundation.Network.Message;
using AsunaClient.Foundation.Network.Message.Indexer;
using AsunaClient.Foundation.Network.Message.Serializer;

namespace AsunaClient.Foundation.Network
{
    public delegate void OnConnectCallbackDelegate(OnConnectResult cr);
    public delegate void OnReceiveNetworkMessageDelegate(NetworkMessage message);
    
    
    public enum NetState
    {
        Ready,
        Connecting,
        Connected,
        Disconnected,
    }

    public enum OnConnectResult
    {
        OK,
        Error
    }

    public static class NetworkMgr 
    {
        #region State
        public static void Init(OnReceiveNetworkMessageDelegate onReceive)
        {
            _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                Blocking = true
            };
            _State = NetState.Ready;
            _ReceiveThread = new Thread(_Receiving);
            _SendThread = new Thread(_Sending);
            _OnReceiveNetworkMessageCallback = onReceive;
        }

        public static void ConnectToAsync(
            string ip, 
            int port, 
            OnConnectCallbackDelegate onConnect)
        {
            if (_State != NetState.Ready)
            {
                XDebug.Warning("net state is not ready!");
                return;
            }

            try
            {
                IPAddress address = IPAddress.Parse(ip);
                IPEndPoint endPoint = new IPEndPoint(address, port);
                _OnConnectCallback = onConnect;
                _Socket.BeginConnect(endPoint, _OnConnect, null);
                _State = NetState.Connecting;
            }
            catch (Exception e)
            {
                XDebug.Error(e.Message);
            }
        }

        private static void _OnConnect(IAsyncResult ar)
        {
            try
            {
                _Socket.EndConnect(ar);
            }
            catch (Exception e)
            {
                XDebug.Error(e.Message);
                _OnConnectCallback?.Invoke(OnConnectResult.Error);
            }
            finally
            {
                _State = NetState.Connected;
                _OnConnectCallback?.Invoke(OnConnectResult.OK);
                _ReceiveThread.Start();
                _SendThread.Start();
            }
        }


        private static void Disconnect()
        {
            XDebug.Info("Disconnect");
            _State = NetState.Disconnected;
            _ReceiveThread.Abort();
            _ReceiveThread = null;
            _SendThread.Abort();
            _SendThread = null;
            _Socket.Close();
            _Socket = null;
        }

        public static void Reset()
        {
            if (_State != NetState.Disconnected)
            {
                XDebug.Warning("please close network before reset!");
                return;
            }
            _State = NetState.Ready;
            _SendEvent.Reset();
            lock (_SendQueue)
            {
                _SendQueue.Clear();
            }
            _ReceiveThread = new Thread(_Receiving);
            _SendThread = new Thread(_Sending);
        }
        
        private static NetState _State;
        private static Socket _Socket;
        private static OnConnectCallbackDelegate _OnConnectCallback;

        #endregion

        #region Sync
        public static void Tick()
        {
            _ProcessEvents();
        }

        private static void _ProcessEvents()
        {
            while(true)
            {
                NetworkEvent e;
                lock(_Events)
                {
                    if (_Events.Count == 0)
                    {
                        break;
                    }
                    e = _Events.Dequeue();
                }

                if (e is NetworkEventReceiveMessage evt)
                {
                    _OnReceiveNetworkMessageCallback?.Invoke(evt.Message);
                }
                else if (e is NetworkEventReceiveException || e is NetworkEventSendException)
                {
                    Disconnect();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            
        }
        
        private static readonly Queue<NetworkEvent> _Events = new Queue<NetworkEvent>();
        #endregion

        #region Receiving

        private static void _OnReceiveException(Exception exception)
        {
            var e = new NetworkEventReceiveException()
            {
                Message = exception.Message
            };
            lock (_Events)
            {
                _Events.Enqueue(e);
            }
        }
        
        private static bool _ReceiveHeader()
        {
            
            _BodySize = 0;
            _BodyType = 0;
            try
            {
                // 这个flag设定能保证返回的数据长度符合要求，不会返回部分
                // https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.socketflags
                var bytesReceived = _Socket.Receive(_HeaderBuffer, 0, HeaderSize, SocketFlags.None);
                if (bytesReceived == 0)
                {
                    _OnReceiveException(new EndOfStreamException());
                    return false;
                }
            }
            catch(Exception exception)
            {
                _OnReceiveException(exception);
                return false;
            }

            _BodySize = BitConverter.ToInt32(_HeaderBuffer, 0);
            _BodyType = BitConverter.ToUInt32(_HeaderBuffer, 4);
            return true;
        }

        private static NetworkMessage _ReceiveBody()
        {
            try
            {
                var bytesReceived = _Socket.Receive(_BodyBuffer, 0, _BodySize, SocketFlags.None);
                if (bytesReceived == 0)
                {
                    _OnReceiveException(new EndOfStreamException());
                    return null;
                }
            }
            catch(Exception exception)
            {
                _OnReceiveException(exception);
                return null;
            }

            var msgType = AssemblyRegisterIndexer.Instance.GetTypeByIndex(_BodyType);
            var message = JsonSerializer.Instance.Deserialize(_BodyBuffer, _BodySize, msgType) as NetworkMessage;
            return message;
        }

        private static void _Receiving()
        {
            while(true)
            {
                if (!_ReceiveHeader())
                {
                    break;
                }
                var message = _ReceiveBody();
                if (message == null)
                {
                    break;
                }
                var e = new NetworkEventReceiveMessage()
                {
                    Message = message
                };
                lock (_Events)
                {
                    _Events.Enqueue(e);    
                }
            }
        }
        
        private const int HeaderSize = 8;
        private const int BodySize = 2048;
        private static Thread _ReceiveThread;
        private static OnReceiveNetworkMessageDelegate _OnReceiveNetworkMessageCallback;
        private static int _BodySize;
        private static uint _BodyType;
        private static readonly byte[] _HeaderBuffer = new byte[HeaderSize];
        private static readonly byte[] _BodyBuffer = new byte[BodySize];
        #endregion

        #region Sending

        private static void _OnSendException(Exception exception)
        {
            var e = new NetworkEventSendException()
            {
                Message = exception.Message
            };
            lock (_Events)
            {
                _Events.Enqueue(e);
            }
        }
        
        public static void Send(NetworkMessage message)
        {
            lock(_SendQueue)
            {
                _SendQueue.Enqueue(message);
            }
            _SendEvent.Set();
        }

        private static void _DoSend(NetworkMessage message)
        {
            var data = JsonSerializer.Instance.Serialize(message);
            var buffer = new byte[data.Length + HeaderSize];
            var bodySize = data.Length;
            var typeIndex = AssemblyRegisterIndexer.Instance.GetIndexByType(message.GetType());
            BitConverter.GetBytes(bodySize).CopyTo(buffer, 0);
            BitConverter.GetBytes(typeIndex).CopyTo(buffer, 4);
            data.CopyTo(buffer, HeaderSize);
            try
            {
                _Socket.Send(buffer, SocketFlags.None);
            }
            catch (Exception e)
            {
                _OnSendException(e);
            }
        }

        private static void _Sending()
        {
            while(true)
            {
                NetworkMessage message;
                lock(_SendQueue)
                {
                    message = _SendQueue.Count == 0 ? null : _SendQueue.Dequeue();
                }

                if (message == null)
                {
                    _SendEvent.Reset();
                    _SendEvent.WaitOne();
                }
                else
                {
                    _DoSend(message);
                }
            }
        }
        
        private static Thread _SendThread;
        private static readonly ManualResetEvent _SendEvent = new ManualResetEvent(false);
        private static readonly Queue<NetworkMessage> _SendQueue = new Queue<NetworkMessage>();
        #endregion

        public static void Release()
        {
            if (_State == NetState.Connected)
            {
                Disconnect();
            }
        }
        

    }
}


