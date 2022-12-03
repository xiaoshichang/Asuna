using AsunaServer.Application.Config;
using AsunaServer.Application.Message;
using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Network;

namespace AsunaServer.Application.Server;

public partial class GMServer  : ServerBase
{
    
    protected override void _OnInnerPing(TcpSession session, InnerPingReq req)
    {
        base._OnInnerPing(session, req);
        var config = _GroupConfig.GetServerConfigByName(req.ServerName);
        if (config == null)
        {
            Logger.Warning("unknown server name");
            return;
        }
        if (config is GameServerConfig)
        {
            _AllGames.Add(session);
        }

        if (config is GateServerConfig)
        {
            _AllGates.Add(session);
        }
    }
    
    
    private void _OnServerReadyNtf(TcpSession session, ServerReadyNtf ntf)
    {
        _ReadyServerCount += 1;
        if (_ReadyServerCount == _GroupConfig.GetServerGroupNodesCount() - 1)
        {
            _OnAllServerReady();
        }
    }

    private void _OnAllServerReady()
    {
        Logger.Info($"_OnAllServerReady. games: {_AllGames.Count}, gates: {_AllGates.Count}");
        _SendStubsDistributeNotifyToGames();
    }

    private int _ReadyServerCount = 0;
    private readonly List<TcpSession> _AllGames = new();
    private readonly List<TcpSession> _AllGates = new();
}