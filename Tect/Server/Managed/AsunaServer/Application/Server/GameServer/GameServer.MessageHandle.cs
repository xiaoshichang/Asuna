using AsunaServer.Message;
using AsunaServer.Network;

#pragma warning disable CS8604

namespace AsunaServer.Application;

public partial class GameServer : ServerBase
{
    protected override void _RegisterMessageHandlers()
    {
        base._RegisterMessageHandlers();
        _RegisterMessageHandler(typeof(StubsDistributeNtf), _OnStubsDistributeNtf);
        _RegisterMessageHandler(typeof(RpcNtf), _OnRpcCall);
    }
}