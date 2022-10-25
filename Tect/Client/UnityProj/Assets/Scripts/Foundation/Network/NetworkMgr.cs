

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Asuna.Foundation;
using Asuna.Foundation.Network;
using UnityEngine;

namespace AsunaClient.Foundation
{

    public enum NetState
    {
        Ready,
        Connecting,
        Connected,
        Disconnected,
    }

    public enum NetworkEventType
    {
        OnRecvMsg,
        OnDisconnectByRemote
    }

    public class NetworkEvent
    {
        public NetworkEventType EventType;
        public Exception OnConnectException;
        public PackageBase RecvPackage;
        public Exception OnDisconnectByRemoteException;
    }

    public static class NetworkMgr
    {
        public static void Init()
        {
            _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                Blocking = true
            };

            _State = NetState.Ready;
            _ReceiveThread = new Thread(Receiving);
            _SendThread = new Thread(Sending);
        }

        public static void ConnectToServer(string ip, int port, Action<Exception> callback)
        {
            if (_State != NetState.Ready)
            {
                Debug.Log("network is not ready");
                return;
            }

            IPAddress address = IPAddress.Parse(ip);
            IPEndPoint lep = new IPEndPoint(address, port);
            _State = NetState.Connecting;
            _OnConnectCallback = callback;
            
            try
            {
                _Socket.Connect(lep);
                Debug.Log($"Connected To Server {ip}:{port}");
                _State = NetState.Connected;
                callback.Invoke(null);
                _ReceiveThread.Start();
                _SendThread.Start();
            }
            catch(Exception e)
            {
                callback.Invoke(e);
            }
        }

        private static void DisconnectByException(Exception exception)
        {
            NetworkEvent evt = new NetworkEvent()
            {
                EventType = NetworkEventType.OnDisconnectByRemote,
                OnDisconnectByRemoteException = exception

            };
            lock(_Events)
            {
                _Events.Enqueue(evt);
            }
        }

        private static bool ReceiveHeader(out int packageType, out int payloadSize)
        {
            packageType = 0;
            payloadSize = 0;
            
            var headerBuffer = new byte[PackageBase.PackageHeaderSize];
            var bytesReceived = 0;
            while (bytesReceived != PackageBase.PackageHeaderSize)
            {
                try
                {
                    bytesReceived += _Socket.Receive(headerBuffer, bytesReceived, PackageBase.PackageHeaderSize - bytesReceived, SocketFlags.None);
                    if (bytesReceived == 0)
                    {
                        DisconnectByException(new EndOfStreamException());
                        
                        return false;
                    }
                }
                catch(Exception exception)
                {
                    DisconnectByException(exception);
                    return false;
                }
            }

            packageType = BitConverter.ToInt32(headerBuffer, 0);
            payloadSize = BitConverter.ToInt32(headerBuffer, 4);
            return true;
        }

        private static PackageBase ReceiveBody(int packageType, int payloadSize)
        {
            PackageBase package;
            if ((PackageType) packageType == PackageType.Json)
            {
                package = new PackageJson()
                {
                    Payload = new byte[payloadSize],
                    PayloadOffset = 0
                };
            }
            else
            {
                throw new Exception("unknown package type");
            }
            
            
            while (package.PayloadOffset != payloadSize)
            {
                try
                {
                    var bytesReceived = _Socket.Receive(package.Payload, package.PayloadOffset, payloadSize - package.PayloadOffset, SocketFlags.None);
                    if (bytesReceived == 0)
                    {
                        DisconnectByException(new EndOfStreamException());
                        break;
                    }
                    package.PayloadOffset += bytesReceived;
                }
                catch(Exception exception)
                {
                    DisconnectByException(exception);
                    break;
                }
            }
            return package;
        }

        private static bool ReceiveBody(int packageType, int payloadSize, out PackageBase package)
        {
            package = null;
            package = ReceiveBody(packageType, payloadSize);
            return true;
        }

        private static void Receiving()
        {
                
            while(true)
            {
                if (!ReceiveHeader(out int packageType, out int payloadSize))
                {
                    break;
                }
                if (!ReceiveBody(packageType, payloadSize, out PackageBase msg))
                {
                    break;
                }
                NetworkEvent e = new NetworkEvent()
                {
                    EventType = NetworkEventType.OnRecvMsg,
                    RecvPackage = msg
                };
                lock(_Events)
                {
                    _Events.Enqueue(e);
                }
               
            }
        }

        public static void Send(PackageBase package)
        {
            lock(_SendQueue)
            {
                _SendQueue.Enqueue(package);
            }
            _SendEvent.Set();
        }

        private static void SendPackage(PackageBase package)
        {
            var buffer = package.DumpPackage();
            int bytesSent = 0;
            while (bytesSent != buffer.Length)
            {
                bytesSent += _Socket.Send(buffer, bytesSent, buffer.Length - bytesSent, SocketFlags.None);
            }
        }

        private static void Sending()
        {
            while(true)
            {
                PackageBase package;
                lock(_SendQueue)
                {
                    package = _SendQueue.Count == 0 ? null : _SendQueue.Dequeue();
                }

                if (package == null)
                {
                    _SendEvent.Reset();
                    _SendEvent.WaitOne();
                }
                else
                {
                    SendPackage(package);
                }
            }
        }

        public static void Tick()
        {
            ProcessNetworkEvents();
        }

        public static void Disconnect()
        {
           _Socket.Close();
        }


        private static void ProcessRecvMsg(PackageBase package)
        {
            OnReceiveMsg?.Invoke(package);
        }

        private static void ProcessNetworkEvents()
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
                switch(e.EventType)
                {
                    case NetworkEventType.OnRecvMsg:
                    {
                        ProcessRecvMsg(e.RecvPackage);
                        break;
                    }
                    case NetworkEventType.OnDisconnectByRemote:
                    {
                        Debug.Log($"OnDisconnectByRemote {e.OnDisconnectByRemoteException.Message}");
                        break;
                    }
                    default:
                    {
                        throw new NotImplementedException(); 
                    }
                }
            }
        }

        public static void OnApplicationQuit()
        {
            _ReceiveThread.Abort();
            _SendThread.Abort();
            _Socket.Close();
        }


        private static Action<Exception> _OnConnectCallback; 
        private static NetState _State;
        private static Socket _Socket;
        private static Thread _ReceiveThread;
        private static Thread _SendThread;
        private static readonly Queue<NetworkEvent> _Events = new Queue<NetworkEvent>();
        private static readonly Queue<PackageBase> _SendQueue = new Queue<PackageBase>();
        private static readonly ManualResetEvent _SendEvent = new ManualResetEvent(false);
        public static Action<PackageBase> OnReceiveMsg;


    }
}