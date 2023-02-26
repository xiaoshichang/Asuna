
using AsunaServer.Application;

namespace AsunaServer.Application
{
    public partial class GMServer  : ServerBase
    {

        public override void Init()
        {
            _RegisterInnerNetworkMessage();
            _RegisterMessageHandlers();
            _RegisterServerStubs();
            _InitCoreAndNetwork();
        }
    }
}