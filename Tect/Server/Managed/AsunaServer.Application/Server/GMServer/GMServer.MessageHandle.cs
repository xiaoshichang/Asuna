using AsunaServer.Application.Server.InnerMessage;
using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Network;

namespace AsunaServer.Application.Server;

#pragma warning disable CS8604

public partial class GMServer  : ServerBase
{
    protected override bool _HandleMessage(TcpSession session, object message, Type type)
    {
        if (base._HandleMessage(session, message, type))
        {
            return true;
        }

        if (type == typeof(ServerReadyNtf))
        {
            var ntf = message as ServerReadyNtf;
            _OnServerReadyNtf(session, ntf);
            return true;
        }

        return false;

    }
}