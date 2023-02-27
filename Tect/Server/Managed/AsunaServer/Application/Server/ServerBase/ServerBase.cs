﻿using System.Reflection;
using AsunaServer.Core;
using AsunaServer.Entity;
using AsunaServer.Debug;
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

        protected void _RegisterRPC()
        {
            var assemblyList = new List<Assembly> { Assembly.GetExecutingAssembly() };
            G.RPCTable.Register(assemblyList);
        }

        protected void _RegisterServerStubs()
        {
            var assemblyList = new List<Assembly> { Assembly.GetExecutingAssembly() };
            EntityMgr.RegisterStubTypes(assemblyList);
        }

        protected void _InitCoreAndNetwork()
        {
            Interface.Server_Init();
            InnerNetwork.Init(G.ServerConfig.InnerIp, G.ServerConfig.InnerPort, _OnNodeAccept, _OnNodeConnect, _OnNodeReceiveMessage, _OnNodeDisconnect);
            Logger.Info($"{G.ServerConfig.Name} listen at {G.ServerConfig.InnerIp}:{G.ServerConfig.InnerPort}");
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

