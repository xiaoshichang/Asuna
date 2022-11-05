using AsunaServer.Core;
using AsunaServer.Foundation.Config;
using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Network;

namespace AsunaServer.Foundation.Server
{
    public abstract partial class ServerBase
    {
        protected ServerBase(ServerGroupConfig groupConfig, ServerConfigBase serverConfig)
        {
            _GroupConfig = groupConfig;
            _ServerConfig = serverConfig;
        }
        
        public virtual void Init()
        {
            Interface.Server_Init();
            InnerNetwork.Init(_ServerConfig.InternalIP, _ServerConfig.InternalPort, OnAccept);
            Logger.Info($"{_ServerConfig.Name} listen at {_ServerConfig.InternalIP}:{_ServerConfig.InternalPort}");
        }

        public virtual void OnAccept(TcpSession session)
        {
        }

        public void Run()
        {
            Interface.Server_Run();
        }

        public virtual void Release()
        {
            Interface.Server_Finalize();
        }

        /// <summary>
        /// 服务器组配置
        /// </summary>
        protected readonly ServerGroupConfig _GroupConfig;
        
        /// <summary>
        /// 本服务器配置
        /// </summary>
        protected readonly ServerConfigBase _ServerConfig;
        
        /// <summary>
        /// 同上
        /// </summary>
        protected readonly Dictionary<string, TcpSession> _ServerToSession = new();
    }
}

