using System;
using System.Collections.Generic;
using Asuna.Foundation.Network;
using Asuna.Foundation.Network.Rpc;

#pragma warning disable CS8604
#pragma warning disable CS8602


namespace Asuna.Foundation.Servers
{
    public delegate void PayloadMsgHandler(TcpSession session, PayloadMsg msg);
    
    
    public abstract class ServerBase
    {
        protected ServerBase(ServerGroupConfig groupConfig, ServerConfigBase serverConfig)
        {
            _ServerGroupConfig = groupConfig;
            _ServerConfig = serverConfig;
        }


        private void _InitNetworkCallback()
        {
            _InternalNetwork.Init(_ServerConfig.InternalIP, _ServerConfig.InternalPort);
            _InternalNetwork.OnAcceptConnectionCallback = _OnInternalAcceptConnection;
            _InternalNetwork.OnDisconnectCallback = _OnInternalDisconnect;
            _InternalNetwork.OnReceivePackageCallback = _OnInternalReceivePackage;
            _InternalNetwork.OnConnectToCallback = _OnInternalConnectTo;
        }

        private void _InitRpc()
        {
            RpcRegister.CollectRpc();
        }
        
        public virtual void Init()
        {
            ALogger.LogInfo($"Server {_ServerConfig.Name} Init!");
            _InitRpc();
            _InitNetworkCallback();
        }

        public virtual void Uninit()
        {
        }

        protected virtual void _ProcessNetworkEvents()
        {
            _InternalNetwork.ProcessNetworkEvents();
        }

        protected virtual void _ProcessTimerEvents()
        {
            TimerMgr.Tick();
        }

        /// <summary>
        /// callback when a internal node accepted
        /// </summary>
        protected virtual void _OnInternalAcceptConnection(NetworkEvent evt)
        {
        }

        /// <summary>
        /// callback when a internal node disconnected
        /// </summary>
        protected virtual void _OnInternalDisconnect(NetworkEvent evt)
        {
            ALogger.LogInfo($"OnInternalDisconnect {evt.Session}");
        }

        protected virtual void _OnControlMsgHandShakeReq(TcpSession session, PayloadMsg msg)
        {
            var req = msg as PayloadMsgHandShakeReq;
            _ServerToSession[req.ServerName] = session;
            var rsp = new PayloadMsgHandShakeRsp(_ServerConfig.Name);
            session.SendPayloadMsg(PayloadMsgType.HandShakeRsp, rsp);
            ALogger.LogInfo($"OnControlMsgHandShakeReq {req.ServerName}");

        }

        protected virtual void _OnControlMsgHandShakeRsp(TcpSession session, PayloadMsg msg)
        {
            var rsp = msg as PayloadMsgHandShakeRsp;
            _ServerToSession[rsp.ServerName] = session;
            ALogger.LogInfo($"OnControlMsgHandShakeRsp {rsp.ServerName}");
        }

        protected virtual void _OnControlMsgConnectGamesNotify(TcpSession session, PayloadMsg msg)
        {
            var games = _ServerGroupConfig.GameServers;
            foreach (var game in games)
            {
                _InternalNetwork.ConnectTo(game.InternalIP, game.InternalPort);
            }
        }

        protected virtual void _OnControlMsgGamesConnectedNotify(TcpSession session, PayloadMsg msg)
        {
            throw new NotImplementedException();
        }

        protected virtual void _OnControlMsgStartupStubs(TcpSession session, PayloadMsg msg)
        {
            throw new NotImplementedException();
        }

        protected virtual void _OnControlMsgStubReady(TcpSession session, PayloadMsg msg)
        {
            var notify = msg as PayloadMsgStubReadyNotify;
            if (notify == null)
            {
                ALogger.LogError("_OnControlMsgStubReady unknown error");
                return;
            }
            if (_StubToSession.ContainsKey(notify.StubName))
            {
                ALogger.LogError("_OnControlMsgStubReady duplicated stub register");
                return;
            }
            _StubToSession[notify.StubName] = session;
        }


        protected virtual void _ProcessPackage(TcpSession session, PackageBase package)
        {
            var msgType = package.GetPayloadMsgType();
            PayloadMsg msg;
            switch ((PayloadMsgType)msgType)
            {
                case PayloadMsgType.HandShakeReq:
                    msg = package.ParsePayload<PayloadMsgHandShakeReq>();
                    _OnControlMsgHandShakeReq(session, msg);
                    return;
                case PayloadMsgType.HandShakeRsp:
                    msg = package.ParsePayload<PayloadMsgHandShakeRsp>();
                    _OnControlMsgHandShakeRsp(session, msg);
                    return;
                case PayloadMsgType.ConnectGamesNotify:
                    msg = package.ParsePayload<PayloadMsgConnectGamesNotify>();
                    _OnControlMsgConnectGamesNotify(session, msg);
                    return;
                case PayloadMsgType.GamesConnectedNotify:
                    msg = package.ParsePayload<PayloadMsgGamesConnectedNotify>();
                    _OnControlMsgGamesConnectedNotify(session, msg);
                    return;
                case PayloadMsgType.StartupStubsNotify:
                    msg = package.ParsePayload<PayloadMsgStartupStubsNotify>();
                    _OnControlMsgStartupStubs(session, msg);
                    return;
                case PayloadMsgType.StubReadyNotify:
                    msg = package.ParsePayload<PayloadMsgStubReadyNotify>();
                    _OnControlMsgStubReady(session, msg);
                    return;
                default:
                    throw new NotImplementedException("unsupported message type!");
            }
        }
        
        /// <summary>
        /// callback when receive a network package from internal network
        /// </summary>
        protected virtual void _OnInternalReceivePackage(NetworkEvent evt)
        {
            if (evt.ReceivedPackage.PackageType == PackageType.Json)
            {
                _ProcessPackage(evt.Session, evt.ReceivedPackage);
            }
            else
            {
                throw new NotImplementedException("Unsupported PackageType");
            }
        }
        
        /// <summary>
        /// callback when connect to a internal node
        /// </summary>
        protected virtual void _OnInternalConnectTo(NetworkEvent evt)
        {
            evt.Session.StartReceiving();
            var req = new PayloadMsgHandShakeReq(_ServerConfig.Name);
            evt.Session.SendPayloadMsg(PayloadMsgType.HandShakeReq, req);
        }


        public virtual void Run()
        {
            _InternalNetwork.StartListen();
            while (!_QuitFlag)
            {
                _ProcessNetworkEvents();
                _ProcessTimerEvents();
                _InternalNetwork.LoopEvent.Reset();
                _InternalNetwork.LoopEvent.WaitOne(10);
            }
        }

        protected bool _QuitFlag = false;
        protected readonly ServerGroupConfig _ServerGroupConfig;
        protected readonly ServerConfigBase _ServerConfig;
        protected readonly NetworkMgrBase _InternalNetwork = new NetworkMgrTcp();
        protected readonly Dictionary<string, TcpSession> _ServerToSession = new();
        protected readonly Dictionary<string, TcpSession> _StubToSession = new();

    }
}
