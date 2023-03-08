
using AsunaServer.Application;

namespace AsunaServer.Application
{
    public partial class GMServer  : ServerBase
    {

        public override void Init()
        {
            _RegisterInnerNetworkMessage();
            _RegisterBasicMessageHandlers();
            _RegisterRpcIndex();
            _RegisterServerStubs();
            _InitCoreAndNetwork();
        }
    }
}