using AsunaServer.Entity;
using AsunaServer.Foundation.Debug;
using AsunaServer.Message;
using AsunaServer.Network;

namespace AsunaServer.Application;

public partial class GateServer : ServerBase
{
    protected void _OnStubRpc(TcpSession session, object message)
    {
        if (message is not StubRpc rpc)
        {
            ADebug.Error($"_OnStubRpc - unknown error.");
            return;
        }

        var game = Sessions.GetSessionByName(rpc.Server);
        if (game == null)
        {
            ADebug.Error($"_OnStubRpc - game {rpc.Server} not found.");
            return;
        }
        
        game.Send(message);
    }
}