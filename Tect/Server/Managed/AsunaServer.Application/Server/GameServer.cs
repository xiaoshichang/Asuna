using AsunaServer.Foundation.Config;

namespace AsunaServer.Foundation.Server
{
    public class GameServer : ServerBase
    {
        public GameServer(ServerGroupConfig groupConfig, GameServerConfig serverConfig) : base(groupConfig, serverConfig)
        {
        }
        
        public override void Init()
        {
            base.Init();
            _TryConnectGMSever();
        }
        
        public override void Release()
        {
            base.Release();
        }
    }
}

