using AsunaServer.Message;
using AsunaServer.Network;

#pragma warning disable CS8604

namespace AsunaServer.Application;

public partial class GameServer : ServerBase
{
    protected override void _RegisterBasicMessageHandlers()
    {
        base._RegisterBasicMessageHandlers();
        _RegisterMessageHandler(typeof(StubsDistributeNtf), _OnStubsDistributeNtf);
        _RegisterMessageHandler(typeof(StubRpc), _OnStubRpc);
        _RegisterMessageHandler(typeof(CreateServerEntityNtf), _OnCreateServerEntity);
    }
}