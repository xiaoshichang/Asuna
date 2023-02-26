
using System.Reflection;
using AsunaServer.Debug;
using AsunaServer.Application;
using AsunaServer.Network;

namespace AsunaServer.Application
{
    public partial class GateServer : ServerBase
    {

        private void _RegisterClientNetworkMessage()
        {
            var assemblyList = new List<Assembly> { Assembly.GetExecutingAssembly() };
            OuterNetwork.Serializer.Collect(assemblyList, "AsunaShared.Message");
            OuterNetwork.Serializer.DebugPrint();
        }
        
        public override void Init()
        {
            _RegisterInnerNetworkMessage();
            _RegisterClientNetworkMessage();
            _RegisterMessageHandlers();
            _RegisterServerStubs();
            _InitCoreAndNetwork();
            _TryConnectGMSever();
        }


    }
}