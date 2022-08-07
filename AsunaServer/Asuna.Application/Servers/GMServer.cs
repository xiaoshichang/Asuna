using System.Collections.Generic;
using Asuna.Foundation;
using Newtonsoft.Json.Serialization;

namespace Asuna.Application
{
    public class GMServer : ServerBase
    {
        public GMServer(ServerGroupConfig groupConfig, GMServerConfig serverConfig) : base(groupConfig, serverConfig)
        {
            TimerMgr.RegisterTimer(2000, _TryConnectAllNodes);
        }
        
        private void _TryConnectAllNodes()
        {
            _InternalNetwork.ConnectTo(_ServerGroupConfig.DBServer.InternalIP, _ServerGroupConfig.DBServer.InternalPort);
            foreach (var config in _ServerGroupConfig.GameServers)
            {
                _InternalNetwork.ConnectTo(config.InternalIP, config.InternalPort);
            }
            foreach (var config in _ServerGroupConfig.GateServers)
            {
                _InternalNetwork.ConnectTo(config.InternalIP, config.InternalPort);
            }
        }

        private void _NotifyDBConnectGames()
        {
            var session = _ServerToSession[_ServerGroupConfig.DBServer.Name];
            var msg = new ControlMsgConnectGamesNotify();
            session.SendMsg(msg);
        }
        
        private void _NotifyGatesConnectGames()
        {
            var gates = _ServerGroupConfig.GateServers;
            var msg = new ControlMsgConnectGamesNotify();
            foreach (var gate in gates)
            {
                var session = _ServerToSession[gate.Name];
                session.SendMsg(msg);
            }
        }
        
        protected override void _OnControlMsgHandShakeRsp(TcpSession session, MsgBase msg)
        {
            base._OnControlMsgHandShakeRsp(session, msg);
            if (_ServerToSession.Count == _ServerGroupConfig.GetServerGroupNodesCount() - 1)
            {
                Logger.LogInfo("all nodes connected!");
                _NotifyDBConnectGames();
                _NotifyGatesConnectGames();
            }
        }
        
        protected override void _OnControlMsgGamesConnectedNotify(TcpSession session, MsgBase msg)
        {
            _ReadyGateAndDBCount += 1;
            if (_ReadyGateAndDBCount == _ServerGroupConfig.GateServers.Count + 1)
            {
                Logger.LogInfo("all gate and db servers ready");
            }
        }

        
        private int _ReadyGateAndDBCount;
    }
}

