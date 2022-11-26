
using AsunaServer.Application.Config;

namespace AsunaServer.Application.Server
{
    public partial class GMServer  : ServerBase
    {
        public GMServer(ServerGroupConfig groupConfig, GMServerConfig serverConfig) : base(groupConfig, serverConfig)
        {
        }


    }
}