

using AsunaServer.Foundation.Config;

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
        }
        
        public override void Release()
        {
            base.Release();
        }
    }
}