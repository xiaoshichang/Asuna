using AsunaServer.Application.Message;
using AsunaServer.Foundation.Network;

#pragma warning disable CS8604

namespace AsunaServer.Application.Server;

public partial class GameServer : ServerBase
{
    protected override bool _HandleMessage(TcpSession session, object message, Type type)
    {
        if (base._HandleMessage(session, message, type))
        {
            return true;
        }

        if (type == typeof(StubsDistributeNtf))
        {
            var ntf = message as StubsDistributeNtf;
            _OnStubsDistributeNtf(session, ntf);
            return true;
        }

        return false;

    }
}