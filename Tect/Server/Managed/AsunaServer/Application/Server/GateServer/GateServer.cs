
using System.Reflection;
using AsunaServer.Foundation.Debug;
using AsunaServer.Application;
using AsunaServer.Network;

namespace AsunaServer.Application
{
    public partial class GateServer : ServerBase
    {

        private void _RegisterClientNetworkMessage()
        {
            var assemblyList = new List<Assembly> { Assembly.GetExecutingAssembly() };
            G.MessageSerializer.Collect(assemblyList, "AsunaShared.Message");
        }
        
        public override void Init()
        {
            _RegisterInnerNetworkMessage();
            _RegisterClientNetworkMessage();
            _RegisterMessageHandlers();
            _RegisterRPC();
            _RegisterServerStubs();
            _InitCoreAndNetwork();
            _TryConnectGMSever();
        }


    }
}