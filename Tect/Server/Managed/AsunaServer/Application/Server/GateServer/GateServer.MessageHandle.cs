using AsunaServer.Message;
using AsunaShared.Message;

#pragma warning disable CS8604

namespace AsunaServer.Application;

public partial class GateServer : ServerBase
{
    protected override void _RegisterBasicMessageHandlers()
    {
        base._RegisterBasicMessageHandlers();
        _RegisterMessageHandler(typeof(OpenGateNtf), _OnOpenGateNtf);
        _RegisterMessageHandler(typeof(AuthReq), _OnLoginReq);
        _RegisterMessageHandler(typeof(StubRpc), _OnStubRpc);
        _RegisterMessageHandler(typeof(AsunaServer.Message.AccountRpc), _OnAccountRpcFromGame);
        _RegisterMessageHandler(typeof(AsunaShared.Message.AccountRpc), _OnAccountRpcFromClient);
        _RegisterMessageHandler(typeof(AsunaServer.Message.CreateServerEntityNtf), _OnCreateServerEntity);
    }
}