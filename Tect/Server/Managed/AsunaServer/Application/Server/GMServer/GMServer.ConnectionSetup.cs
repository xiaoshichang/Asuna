using AsunaServer.Application;
using AsunaServer.Message;
using AsunaServer.Debug;
using AsunaServer.Network;

namespace AsunaServer.Application;

public partial class GMServer  : ServerBase
{
    
    protected override void _OnInnerPing(TcpSession session, object message)
    {
        base._OnInnerPing(session, message);
        var req = message as InnerPingReq;
        if (req == null)
        {
            throw new ArgumentException();
        }
        var config = G.GroupConfig.GetServerConfigByName(req.ServerName);
        if (config == null)
        {
            Logger.Warning("unknown server name");
            return;
        }
    }
    
    
    private void _OnServerReadyNtf(TcpSession session, object message)
    {
        var ntf = message as ServerReadyNtf;
        _ReadyServerCount += 1;
        if (_ReadyServerCount == G.GroupConfig.GetServerGroupNodesCount() - 1)
        {
            _OnAllServerReady();
        }
    }

    private void _OnAllServerReady()
    {
        Logger.Info($"_OnAllServerReady. games: {G.Games.Count}, gates: {G.Gates.Count}");
        _SendStubsDistributeNotifyToGames();
    }

    private int _ReadyServerCount = 0;
}