
using System.Reflection;
using AsunaServer.Debug;
using AsunaServer.Application;
using AsunaServer.Network;

namespace AsunaServer.Application
{
    public partial class GateServer : ServerBase
    {
        public GateServer(ServerGroupConfig groupConfig, GateServerConfig serverConfig) : base(groupConfig, serverConfig)
        {
        }

        private void _RegisterClientNetworkMessage()
        {
            var assemblyList = new List<Assembly> { Assembly.GetExecutingAssembly() };
            OuterNetwork.Serializer.Collect(assemblyList, "AsunaShared.Message");
            OuterNetwork.Serializer.DebugPrint();
        }
        
        public override void Init()
        {
            base.Init();
            _RegisterClientNetworkMessage();
            _TryConnectGMSever();
        }


    }
}