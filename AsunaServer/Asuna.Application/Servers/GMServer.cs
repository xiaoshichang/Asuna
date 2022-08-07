using System.Collections.Generic;
using Asuna.Foundation;
using Newtonsoft.Json.Serialization;

namespace Asuna.Application
{
    public sealed class GMServer : ServerBase
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
                Logger.LogInfo("all nodes connected");
                _NotifyDBConnectGames();
                _NotifyGatesConnectGames();
            }
        }

        private void _StartupAllStubs()
        {
            var assemblyList = new List<string>()
            {
                "Asuna.GamePlay"
            };
            var table = ServerStubDistributeTable.Collect(assemblyList, _ServerGroupConfig.GameServers);
            _StubCount = table.Items.Count;
            var msg = new ControlMsgStartupStubsNotify(table);
            foreach (var game in _ServerGroupConfig.GameServers)
            {
                var session = _ServerToSession[game.Name];
                session.SendMsg(msg);
            }
        }
        
        protected override void _OnControlMsgGamesConnectedNotify(TcpSession session, MsgBase msg)
        {
            _ReadyGateAndDBCount += 1;
            if (_ReadyGateAndDBCount == _ServerGroupConfig.GateServers.Count + 1)
            {
                Logger.LogInfo("all gate and db servers ready");
                _StartupAllStubs();
            }
        }
        
        protected override void _OnControlMsgStubReady(TcpSession session, MsgBase msg)
        {
            base._OnControlMsgStubReady(session, msg);
            _ReadyStubCount += 1;
            if (_ReadyStubCount == _StubCount)
            {
                Logger.LogInfo("all stubs are ready");
            }
        }

        
        private int _ReadyGateAndDBCount;
        private int _StubCount;
        private int _ReadyStubCount;

    }
}

