
using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Config;
using AsunaServer.Foundation.Network;

namespace AsunaServer.Foundation.Server
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
        
        public override void Release()
        {
            base.Release();
        }
    }
}