
using AsunaServer.Foundation.Log;
using AsunaServer.Application.Config;
using AsunaServer.Foundation.Network;

namespace AsunaServer.Application.Server
{
    public class GateServer  : ServerBase
    {
        public GateServer(ServerGroupConfig groupConfig, GateServerConfig serverConfig) : base(groupConfig, serverConfig)
        {
        }
        
        public override void Init()
        {
            base.Init();
            _TryConnectGMSever();
        }

        protected override void _OnInnerPing(TcpSession session, InnerPingReq req)
        {
            base._OnInnerPing(session, req);
            var config = _GroupConfig.GetServerConfigByName(req.ServerName);
            if (config == null)
            {
                Logger.Warning("unknown server name");
                return;
            }
            if (config is GameServerConfig)
            {
                _ConnectedGames += 1;
                if (_ConnectedGames == _GroupConfig.GameServers.Count)
                {
                    _OnAllGamesConnected();
                }
            }
        }

        private void _OnAllGamesConnected()
        {
            Logger.Info("_OnAllGamesConnected");
        }
        
        public override void Release()
        {
            base.Release();
        }

        private int _ConnectedGames = 0;
    }
}