using AsunaServer.Message;
using AsunaServer.Network;

#pragma warning disable CS8604

namespace AsunaServer.Application;

public partial class GameServer : ServerBase
{
    protected override bool _HandleMessage(TcpSession session, object message)
    {
        if (base._HandleMessage(session, message))
        {
            return true;
        }

        var type = message.GetType();
        if (type == typeof(StubsDistributeNtf))
        {
            var ntf = message as StubsDistributeNtf;
            _OnStubsDistributeNtf(session, ntf);
            return true;
        }

        return false;

    }
}