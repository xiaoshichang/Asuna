
using AsunaServer.Application.Config;

namespace AsunaServer.Application.Server
{
    public class GMServer  : ServerBase
    {
        public GMServer(ServerGroupConfig groupConfig, GMServerConfig serverConfig) : base(groupConfig, serverConfig)
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