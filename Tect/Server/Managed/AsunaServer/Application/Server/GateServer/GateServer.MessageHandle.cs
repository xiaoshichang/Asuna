using AsunaServer.Message;
using AsunaServer.Network;
using AsunaShared.Message;

#pragma warning disable CS8604

namespace AsunaServer.Application;

public partial class GateServer : ServerBase
{
    protected override void _RegisterMessageHandlers()
    {
        base._RegisterMessageHandlers();
        _RegisterMessageHandler(typeof(OpenGateNtf), _OnOpenGateNtf);
        _RegisterMessageHandler(typeof(LoginReq), _OnLoginReq);
    }
}