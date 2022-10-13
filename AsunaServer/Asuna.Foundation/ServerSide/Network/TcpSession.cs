using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Asuna.Foundation.Network;


#pragma warning disable CS8602
#pragma warning disable CS8618

namespace Asuna.Foundation
{
    public class TcpSession
    {
        public TcpSession(Socket socket, NetworkEventHandler eventHandler)
        {
            _Socket = socket;
            _OnEventCallback = eventHandler;
        }

        public void StartReceiving()
        {
            StartReceiveHeader();
        }

        private void StartReceiveHeader()
        {
            _Socket.BeginReceive(_HeaderBuffer, _HeaderOffset, PackageBase.PackageHeaderSize, SocketFlags.None, OnReceiveHeader, null);
        }

        /// <summary>
        /// callback when receive some bytes of message header.
        /// note that this is called by other thread than main thread.
        /// </summary>
        private void OnReceiveHeader(IAsyncResult ar)
        {
            try
            {
                var receiveSize = _Socket.EndReceive(ar);
                if (receiveSize == 0)
                {
                    DisconnectFromSession(DisconnectReason.CloseByRemote);
                    return;
                }
            
                if (receiveSize == PackageBase.PackageHeaderSize)
                {
                    PackageBase.ParseHeader(_HeaderBuffer, out PackageType packageType, out int payloadSize);
                    _HeaderOffset = 0;
                    StartReceivePayload(packageType, payloadSize);
                }
                else if (receiveSize < PackageBase.PackageHeaderSize)
                {
                    _HeaderOffset += receiveSize;
                    StartReceiveHeader();
                }
                else
                {
                    DisconnectFromSession(DisconnectReason.UnknownError);
                }
            }
            catch (Exception e)
            {
                ALogger.LogError($"OnReceiveHeader Exception {e.Message}");
                DisconnectFromSession(DisconnectReason.UnknownError);
            }
        }

        private void StartReceivePayload(PackageBase package)
        {
            _Socket.BeginReceive(package.Payload, package.PayloadOffset, package.PayloadSize - package.PayloadOffset, SocketFlags.None, OnReceivePayload, package);
        }
        
        /// <summary>
        /// callback when receive some bytes of json body.
        /// note that this is called by other thread than main thread.
        /// </summary>
        private void OnReceivePayload(IAsyncResult ar)
        {
            try
            {
                var receiveSize = _Socket.EndReceive(ar);
                if (receiveSize == 0)
                {
                    DisconnectFromSession(DisconnectReason.CloseByRemote);
                    return;
                }
                var package = ar.AsyncState as PackageBase;
                if (package == null)
                {
                    DisconnectFromSession(DisconnectReason.UnknownError);
                    return;
                }
                package.PayloadOffset += receiveSize;
                if (package.PayloadOffset == package.Payload.Length)
                {
                    var evt = new NetworkEvent()
                    {
                        Session = this,
                        EventType = NetworkEventType.OnReceive,
                        ReceivedPackage = package
                    };
                    _OnEventCallback?.Invoke(evt);
                    StartReceiveHeader();
                }
                else if (package.PayloadOffset < package.Payload.Length)
                {
                    StartReceivePayload(package);
                }
                else
                {
                    DisconnectFromSession(DisconnectReason.UnknownError);
                }
            }
            catch (Exception e)
            {
                ALogger.LogError($"OnReceiveJsonMsg Exception {e.Message}");
                DisconnectFromSession(DisconnectReason.UnknownError);
            }
        }
        
        private void StartReceivePayload(PackageType packageType, int payloadSize)
        {
            if (packageType == PackageType.Json)
            {
                var jsonPackage = new PackageJson()
                {
                    PackageType = packageType,
                    PayloadSize = payloadSize,
                    Payload = new byte[payloadSize],
                    PayloadOffset = 0
                };
                StartReceivePayload(jsonPackage);
            }
            else
            {
                DisconnectFromSession(DisconnectReason.UnknownError);
            }
        }

        private void DisconnectFromSession(DisconnectReason reason)
        {
            var evt = new NetworkEvent
            {
                Session = this,
                EventType = NetworkEventType.Disconnect,
                DisconnectReason = reason
            };
            _OnEventCallback?.Invoke(evt);
        }

        public void DoDisconnect()
        {
            _Socket.Shutdown(SocketShutdown.Both);
            _Socket.Close();
        }

        /// <summary>
        /// send message to client side.
        /// note that this function must be called from main thread.
        /// </summary>
        public void SendPackage(PackageBase package)
        {

            lock (_SendQueue)
            {
                _SendQueue.Enqueue(package);
                if (_SendQueue.Count == 1)
                {
                    var msgToSend = _SendQueue.Peek();
                    DoSend(msgToSend);
                }
            }
        }

        public void SendPayloadMsg(PayloadMsgType msgType, PayloadMsg msg)
        {
            var package = new PackageJson
            {
                JsonObjectType = (int)msgType,
                JsonObject = msg
            };
            SendPackage(package);
        }

        private void DoSend(PackageBase package)
        {
            _SendBuffer = package.DumpPackage();
            _SendBufferOffset = 0;
            _Socket.BeginSend(_SendBuffer, _SendBufferOffset, _SendBuffer.Length - _SendBufferOffset, SocketFlags.None, OnSend, package);
        }

        private void OnSend(IAsyncResult ar)
        {
            var sent = _Socket.EndSend(ar);
            var package = ar.AsyncState as PackageBase;
            _SendBufferOffset += sent;
            if (_SendBufferOffset == _SendBuffer.Length)
            {
                lock (_SendQueue)
                {
                    _SendQueue.Dequeue();
                    if (_SendQueue.Count > 0)
                    {
                        var msgToSend = _SendQueue.Peek();
                        DoSend(msgToSend);
                    }
                }
            }
            else if (_SendBufferOffset < _SendBuffer.Length)
            {
                _Socket.BeginSend(_SendBuffer, _SendBufferOffset, _SendBuffer.Length - _SendBufferOffset, SocketFlags.None, OnSend, package);
            }
            else
            {
                DisconnectFromSession(DisconnectReason.UnknownError);
            }
        }

        private readonly Socket _Socket;
        private readonly byte[] _HeaderBuffer = new byte[PackageBase.PackageHeaderSize];
        private int _HeaderOffset = 0;
        private byte[] _SendBuffer;
        private int _SendBufferOffset;
        private readonly Queue<PackageBase> _SendQueue = new();
        private readonly NetworkEventHandler _OnEventCallback;
    }
}