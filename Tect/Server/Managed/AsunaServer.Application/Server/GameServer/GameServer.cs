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
            _ServerStubsRegister();

            _TryConnectGMSever();
            _TryConnectGateServer();
        }


    }
}

