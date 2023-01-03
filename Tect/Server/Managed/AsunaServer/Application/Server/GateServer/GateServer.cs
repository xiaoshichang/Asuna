
using AsunaServer.Debug;
using AsunaServer.Application;
using AsunaServer.Network;

namespace AsunaServer.Application
{
    public partial class GateServer : ServerBase
    {
        public GateServer(ServerGroupConfig groupConfig, GateServerConfig serverConfig) : base(groupConfig, serverConfig)
        {
        }
        
        public override void Init()
        {
            base.Init();
            _TryConnectGMSever();
        }


    }
}