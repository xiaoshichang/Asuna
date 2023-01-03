using System.Reflection;
using AsunaServer.Core;
using AsunaServer.Application;
using AsunaServer.Entity;
using AsunaServer.Logger;
using AsunaServer.Network;
using AsunaServer.Message;
using AsunaServer.Message.Serializer;

namespace AsunaServer.Application
{
    public abstract partial class ServerBase
    {
        protected ServerBase(ServerGroupConfig groupConfig, ServerConfigBase serverConfig)
        {
            _GroupConfig = groupConfig;
            _ServerConfig = serverConfig;
        }

        private void _RegisterNetworkMessage()
        {
            var assemblyList = new List<Assembly> { Assembly.GetExecutingAssembly() };
            InnerNetwork.Serializer.Collect(assemblyList);
            InnerNetwork.Serializer.DebugPrint();
        }

        protected void _ServerStubsRegister()
        {
            var assemblyList = new List<Assembly> { Assembly.GetExecutingAssembly() };
            EntityMgr.RegisterStubs(assemblyList);
        }

        private void _InitCoreAndNetwork()
        {
            Interface.Server_Init();
            InnerNetwork.Init(_ServerConfig.InnerIp, _ServerConfig.InnerPort, null, _OnConnect, _OnReceiveMessage);
            Logger.Info($"{_ServerConfig.Name} listen at {_ServerConfig.InnerIp}:{_ServerConfig.InnerPort}");
        }
        
        public virtual void Init()
        {
            _RegisterNetworkMessage();
            _InitCoreAndNetwork();
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

    }
}

