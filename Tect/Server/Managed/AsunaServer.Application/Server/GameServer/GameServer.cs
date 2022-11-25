using AsunaServer.Application.Config;
using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Network;
using AsunaServer.Foundation.Timer;

namespace AsunaServer.Application.Server
{
    public partial class GameServer : ServerBase
    {
        public GameServer(ServerGroupConfig groupConfig, GameServerConfig serverConfig) : base(groupConfig, serverConfig)
        {
        }
        
        public override void Init()
        {
            base.Init();
            _TryConnectGMSever();
            _TryConnectGateServer();
        }

        protected override void _OnInnerPong(TcpSession session, InnerPongRsp rsp)
        {
            base._OnInnerPong(session, rsp);
            var config = _GroupConfig.GetServerConfigByName(rsp.ServerName);
            if (config == null)
            {
                Logger.Warning("unknown server name");
                return;
            }
            if (config is GateServerConfig)
            {
                _ConnectedGates += 1;
                if (_ConnectedGates == _GroupConfig.GateServers.Count)
                {
                    OnAllGatesConnected();
                }
            }
        }

        private void OnAllGatesConnected()
        {
            Logger.Info("OnAllGatesConnected");
        }
        
        public override void Release()
        {
            base.Release();
        }

        private int _ConnectedGates = 0;
    }
}

