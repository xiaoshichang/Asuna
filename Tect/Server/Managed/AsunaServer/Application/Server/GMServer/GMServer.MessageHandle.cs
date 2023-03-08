using AsunaServer.Message;
using AsunaServer.Network;

namespace AsunaServer.Application;

#pragma warning disable CS8604

public partial class GMServer  : ServerBase
{
    protected override void _RegisterBasicMessageHandlers()
    {
        base._RegisterBasicMessageHandlers();
        _RegisterMessageHandler(typeof(ServerReadyNtf), _OnServerReadyNtf);
        _RegisterMessageHandler(typeof(StubReadyNtf), _OnStubReady);
    }
}