using AsunaServer.Application.Message;
using AsunaServer.Foundation.Network;

#pragma warning disable CS8604

namespace AsunaServer.Application.Server;

public partial class GateServer : ServerBase
{
    protected override bool _HandleMessage(TcpSession session, object message, Type type)
    {
        if (base._HandleMessage(session, message, type))
        {
            return true;
        }

        if (type == typeof(OpenGateNtf))
        {
            var ntf = message as OpenGateNtf;
            _OnOpenGateNtf(session, ntf);
            return true;
        }

        return false;
    }
}