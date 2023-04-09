using System.Reflection;
using AsunaServer.Core;
using AsunaServer.Entity;
using AsunaServer.Foundation.Debug;
using AsunaServer.Network;

namespace AsunaServer.Application
{
    public abstract partial class ServerBase
    {
        protected void _RegisterInnerNetworkMessage()
        {
            var assemblyList = new List<Assembly> { Assembly.GetExecutingAssembly() };
            G.MessageSerializer.Collect(assemblyList, "AsunaServer.Message");
        }

        protected void _RegisterRpcIndex()
        {
            var assemblyList = new List<Assembly> { Assembly.GetExecutingAssembly() };
            RpcTable.Register(assemblyList);
        }

        protected void _RegisterServerStubs()
        {
            var assemblyList = new List<Assembly> { Assembly.GetExecutingAssembly() };
            EntityMgr.RegisterStubTypes(assemblyList);
        }

        protected void _RegisterServerEntityTypes()
        {
            var assemblyList = new List<Assembly>();
            if (string.IsNullOrEmpty(G.GroupConfig.Common.GameplayAssembly))
            {
                ADebug.Error("GameplayAssembly is null");
                return;
            }

            try
            {
                var path = Path.Join(Environment.CurrentDirectory, G.GroupConfig.Common.GameplayAssembly);
                assemblyList.Add(Assembly.LoadFrom(path));
                EntityMgr.RegisterServerEntityTypes(assemblyList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected void _InitCoreAndNetwork()
        {
            Interface.Server_Init();
            InnerNetwork.Init(G.ServerConfig.InnerIp, G.ServerConfig.InnerPort, _OnNodeAccept, _OnNodeConnect, _OnNodeReceiveMessage, _OnNodeDisconnect);
            ADebug.Info($"{G.ServerConfig.Name} listen at {G.ServerConfig.InnerIp}:{G.ServerConfig.InnerPort}");
        }

        public abstract void Init();

        public void Run()
        {
            Interface.Server_Run();
        }

        public virtual void Release()
        {
            Interface.Server_Finalize();
        }

        

    }
}

