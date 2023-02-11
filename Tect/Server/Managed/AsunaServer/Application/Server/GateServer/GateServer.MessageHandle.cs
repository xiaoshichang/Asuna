using AsunaServer.Message;
using AsunaServer.Network;
using AsunaShared.Message;

#pragma warning disable CS8604

namespace AsunaServer.Application;

public partial class GateServer : ServerBase
{
    protected override bool _HandleMessage(TcpSession session, object message)
    {
        if (base._HandleMessage(session, message))
        {
            return true;
        }

        if (message is OpenGateNtf ntf)
        {
            _OnOpenGateNtf(session, ntf);
            return true;
        }

        if (message is LoginReq req)
        {
            _OnLoginReq(session, req);
            return true;
        }

        return false;
    }
}