using AsunaServer.Application.Message;
using AsunaServer.Foundation.Network;

namespace AsunaServer.Application.Server;

#pragma warning disable CS8604

public partial class GMServer  : ServerBase
{
    protected override bool _HandleMessage(TcpSession session, object message)
    {
        if (base._HandleMessage(session, message))
        {
            return true;
        }

        var type = message.GetType();
        if (type == typeof(ServerReadyNtf))
        {
            var ntf = message as ServerReadyNtf;
            _OnServerReadyNtf(session, ntf);
            return true;
        }

        if (type == typeof(StubReadyNtf))
        {
            var ntf = message as StubReadyNtf;
            _OnStubReady(session, ntf);
            return true;
        }

        return false;

    }
}