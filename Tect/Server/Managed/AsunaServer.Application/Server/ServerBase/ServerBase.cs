using System.Reflection;
using AsunaServer.Core;
using AsunaServer.Application.Config;
using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Network;
using AsunaServer.Foundation.Network.Message.Indexer;

namespace AsunaServer.Application.Server
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
            var assemblyList = new List<Assembly>();
            assemblyList.Add(Assembly.GetExecutingAssembly());
            AssemblyRegisterIndexer.Instance.Init(assemblyList);
            
            Interface.Server_Init();
            InnerNetwork.Init(_ServerConfig.InternalIP, _ServerConfig.InternalPort, null, _OnReceiveMessage);
            Logger.Info($"{_ServerConfig.Name} listen at {_ServerConfig.InternalIP}:{_ServerConfig.InternalPort}");
        }

        protected void _OnReceiveMessage(TcpSession session, object message, Type type)
        {
            if (type == typeof(InnerPingReq))
            {
                var req = message as InnerPingReq;
                if (req is null)
                {
                    return;
                }
                Logger.Debug($"hello from {req.ServerName}");
            }
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

