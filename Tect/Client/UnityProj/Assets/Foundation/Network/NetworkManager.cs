using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using AsunaClient.Foundation.Message.Serializer;
using AsunaClient.Foundation.Interface;

namespace AsunaClient.Foundation.Network
{
    public delegate void OnConnectCallbackDelegate(OnConnectResult cr);
    public delegate void OnReceiveNetworkMessageDelegate(object message);
    
    
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

    public class NetworkManagerInitParam
    {
        public OnReceiveNetworkMessageDelegate OnReceive;
    }

    public class NetworkManager : IManager
    {
        #region State

        public void Init(object param)
        {
            var initParam = param as NetworkManagerInitParam;
            if (initParam is null)
            {
                throw new Exception("init param is null");
            }
            
            _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                Blocking = true
            };
            _State = NetState.Ready;
            _ReceiveThread = new Thread(_Receiving);
            _SendThread = new Thread(_Sending);
            _OnReceiveNetworkMessageCallback = initParam.OnReceive;
        }

        public void ConnectToAsync(
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

        private void _OnConnect(IAsyncResult ar)
        {
            try
            {
                _Socket.EndConnect(ar);
                var evt = new NetworkEventOnConnect()
                {
                    Result = OnConnectResult.OK
                };
                lock (_Events)
                {
                    _Events.Enqueue(evt);
                }
            }
            catch (Exception e)
            {
                var evt = new NetworkEventOnConnect()
                {
                    Result = OnConnectResult.Error,
                    Message = e.Message
                };
                lock (_Events)
                {
                    _Events.Enqueue(evt);
                }
            }
        }


        private void Disconnect()
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

        public void Reset()
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
        public void Tick()
        {
            _ProcessEvents();
        }

        private void _ProcessEvents()
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
                else if (e is NetworkEventReceiveException receiveException)
                {
                    XDebug.Error(receiveException.Message);
                    Disconnect();
                }
                else if (e is NetworkEventSendException sendException)
                {
                    XDebug.Error(sendException.Message);
                    Disconnect();
                }
                else if (e is NetworkEventOnConnect evtOnConnect)
                {
                    if (evtOnConnect.Result == OnConnectResult.OK)
                    {
                        _State = NetState.Connected;
                        _OnConnectCallback?.Invoke(OnConnectResult.OK);
                        _ReceiveThread.Start();
                        _SendThread.Start();
                    }
                    else if (evtOnConnect.Result == OnConnectResult.Error)
                    {
                        XDebug.Error(evtOnConnect.Message);
                        _OnConnectCallback?.Invoke(OnConnectResult.Error);
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            
        }
        
        private readonly Queue<NetworkEvent> _Events = new Queue<NetworkEvent>();
        #endregion

        #region Receiving

        private void _OnReceiveException(Exception exception)
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
        
        private bool _ReceiveHeader()
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

        private object _ReceiveBody()
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

            var message = Serializer.Deserialize(_BodyBuffer, _BodySize, _BodyType);
            return message;
        }

        private void _Receiving()
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
        private Thread _ReceiveThread;
        private OnReceiveNetworkMessageDelegate _OnReceiveNetworkMessageCallback;
        private int _BodySize;
        private uint _BodyType;
        private readonly byte[] _HeaderBuffer = new byte[HeaderSize];
        private readonly byte[] _BodyBuffer = new byte[BodySize];
        #endregion

        #region Sending

        private void _OnSendException(Exception exception)
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
        
        public void Send(object message)
        {
            lock(_SendQueue)
            {
                _SendQueue.Enqueue(message);
            }
            _SendEvent.Set();
        }

        private void _DoSend(object message)
        {
            var data = Serializer.Serialize(message);
            var buffer = new byte[data.Length + HeaderSize];
            var bodySize = data.Length;
            var typeIndex = Serializer.GetIndexByType(message.GetType());
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

        private void _Sending()
        {
            while(true)
            {
                object message;
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
        
        private Thread _SendThread;
        private readonly ManualResetEvent _SendEvent = new ManualResetEvent(false);
        private readonly Queue<object> _SendQueue = new Queue<object>();
        #endregion

        public void Release()
        {
            if (_State == NetState.Connected)
            {
                Disconnect();
            }
        }

        public readonly SerializerBase Serializer = new ProtobufSerializer();
    }
}


