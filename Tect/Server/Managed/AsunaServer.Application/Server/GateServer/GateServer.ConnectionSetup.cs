using AsunaServer.Application.Config;
using AsunaServer.Application.Server.InnerMessage;
using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Network;
using AsunaServer.Foundation.Timer;

namespace AsunaServer.Application.Server;

#pragma warning disable CS8602

public partial class GateServer : ServerBase
{
    protected void _TryConnectGMSever()
    {
        TimerMgr.AddTimer((uint)Random.Shared.Next(1000, 2000), _DoConnectGMServer);
    }

    private void _DoConnectGMServer(object? param)
    {
        var config = _GroupConfig.GetGMConfig();
        InnerNetwork.ConnectTo(config.InternalIP, config.InternalPort);
    }
    
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
            _ConnectedGames += 1;
            if (_ConnectedGames == _GroupConfig.GameServers.Count)
            {
                _OnAllGamesConnected();
            }
        }
    }

    protected override void _OnInnerPong(TcpSession session, InnerPongRsp rsp)
    {
        base._OnInnerPong(session, rsp);
        var config = _GroupConfig.GetServerConfigByName(rsp.ServerName);
        if (config == null)
        {
            Logger.Warning("unknown server name");
            return;
        }

        if (config is GMServerConfig)
        {
            _GMSession = session;
        }
    }

    private void _OnAllGamesConnected()
    {
        Logger.Info("_OnAllGamesConnected");
        var ntf = new ServerReadyNtf()
        {
            ServerName = _ServerConfig.Name
        };
        _GMSession.Send(ntf);
    }
    
    private void _CallGM(INetworkMessage message)
    {
        _GMSession.Send(message);
    }
    
    private int _ConnectedGames = 0;
    private TcpSession? _GMSession;

}