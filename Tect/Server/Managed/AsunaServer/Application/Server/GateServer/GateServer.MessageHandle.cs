using AsunaServer.Message;
using AsunaServer.Network;
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
        _RegisterMessageHandler(typeof(AccountRpc), _OnAccountRpc);
    }
}