using AsunaServer.Application.Config;
using AsunaServer.Application.Server.InnerMessage;
using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Network;
using AsunaServer.Foundation.Timer;

namespace AsunaServer.Application.Server;

#pragma warning disable CS8602

public partial class GameServer : ServerBase
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
    
    protected void _TryConnectGateServer()
    {
        TimerMgr.AddTimer((uint)Random.Shared.Next(2000, 3000), _DoConnectGateServer);
    }
        
    private void _DoConnectGateServer(object? param)
    {
        foreach (var gate in _GroupConfig.GateServers)
        {
            InnerNetwork.ConnectTo(gate.InternalIP, gate.InternalPort);
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
        if (config is GateServerConfig)
        {
            _ConnectedGates += 1;
            if (_ConnectedGates == _GroupConfig.GateServers.Count)
            {
                OnAllGatesConnected();
            }
        }
        if (config is GMServerConfig)
        {
            _GMSession = session;
        }
    }

    private void OnAllGatesConnected()
    {
        Logger.Info("OnAllGatesConnected");
        var ntf = new ServerReadyNtf()
        {
            ServerName = _ServerConfig.Name
        };
        _GMSession.Send(ntf);
    }
    
    private TcpSession? _GMSession;
    private int _ConnectedGates = 0;

}