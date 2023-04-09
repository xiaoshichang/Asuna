using AsunaServer.Message;
using AsunaServer.Network;
using AsunaServer.Utils;

#pragma warning disable CS8604

namespace AsunaServer.Application;

public partial class GateServer : ServerBase
{
    protected void _OnCreateServerEntity(TcpSession session, object message)
    {
        var ntf = message as CreateServerEntityNtf;
        var game = RandomUtils.RandomGetItem(Sessions.Games.Values.ToArray());
        game.Send(ntf);
    }
    
}