
using AsunaServer.Foundation.Log;
using AsunaServer.Application.Config;
using AsunaServer.Foundation.Network;

namespace AsunaServer.Application.Server
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