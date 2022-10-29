using AsunaServer.Core;
using AsunaServer.Foundation.Config;
using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Network;

namespace AsunaServer.Foundation.Server
{
    public abstract class ServerBase
    {
        public ServerBase(ServerGroupConfig groupConfig, ServerConfigBase serverConfig)
        {
            _GroupConfig = groupConfig;
            _ServerConfig = serverConfig;
        }
        
        public virtual void Init()
        {
            Interface.Server_Init();
            InnerNetwork.Init(_ServerConfig.InternalIP, _ServerConfig.InternalPort);
            Logger.Info($"{_ServerConfig.Name} listen at {_ServerConfig.InternalIP}:{_ServerConfig.InternalPort}");
        }

        public void Run()
        {
            Interface.Server_Run();
        }

        public virtual void Release()
        {
            Interface.Server_Finalize();
        }

        protected readonly ServerGroupConfig _GroupConfig;
        protected readonly ServerConfigBase _ServerConfig;
    }
}

