using AsunaServer.Application;
using AsunaServer.Foundation.Debug;
using AsunaServer.Network;
using AsunaServer.Timer;

namespace AsunaServer.Application
{
    public partial class GameServer : ServerBase
    {
        
        public override void Init()
        {
            _RegisterInnerNetworkMessage();
            _RegisterBasicMessageHandlers();
            _RegisterRpcIndex();
            _RegisterServerStubs();
            _InitCoreAndNetwork();
            _TryConnectGMSever();
        }


    }
}

