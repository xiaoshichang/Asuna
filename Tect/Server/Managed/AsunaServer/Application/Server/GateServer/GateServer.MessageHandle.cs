using AsunaServer.Message;
using AsunaServer.Network;

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

        if (message.GetType() == typeof(OpenGateNtf))
        {
            var ntf = message as OpenGateNtf;
            _OnOpenGateNtf(session, ntf);
            return true;
        }

        return false;
    }
}