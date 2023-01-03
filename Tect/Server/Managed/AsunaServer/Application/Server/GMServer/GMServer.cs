
using AsunaServer.Application;

namespace AsunaServer.Application
{
    public partial class GMServer  : ServerBase
    {
        public GMServer(ServerGroupConfig groupConfig, GMServerConfig serverConfig) : base(groupConfig, serverConfig)
        {
        }

        public override void Init()
        {
            base.Init();
            _ServerStubsRegister();
        }

    }
}