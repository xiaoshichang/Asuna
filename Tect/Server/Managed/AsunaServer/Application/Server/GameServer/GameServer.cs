using AsunaServer.Application;
using AsunaServer.Logger;
using AsunaServer.Network;
using AsunaServer.Timer;

namespace AsunaServer.Application
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
        }


    }
}

