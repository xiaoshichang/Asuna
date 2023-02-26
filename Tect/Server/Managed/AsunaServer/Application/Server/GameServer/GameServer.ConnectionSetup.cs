using AsunaServer.Application;
using AsunaServer.Debug;
using AsunaServer.Message;
using AsunaServer.Network;
using AsunaServer.Timer;

namespace AsunaServer.Application;

#pragma warning disable CS8602

public partial class GameServer : ServerBase
{
    protected void _TryConnectGMSever()
    {
        TimerMgr.AddTimer((uint)Random.Shared.Next(1000, 2000), _DoConnectGMServer);
    }

    private void _DoConnectGMServer(object? param)
    {
        var config = G.GroupConfig.GetGMConfig();
        InnerNetwork.ConnectTo(config.InnerIp, config.InnerPort);
    }
    
    protected void _TryConnectGateServer()
    {
        TimerMgr.AddTimer((uint)Random.Shared.Next(1000, 2000), _DoConnectGateServer);
    }
        
    private void _DoConnectGateServer(object? param)
    {
        foreach (var gate in G.GroupConfig.GateServers)
        {
            InnerNetwork.ConnectTo(gate.InnerIp, gate.InnerPort);
        }
    }
    
    protected override void _OnInnerPong(TcpSession session, object message)
    {
        base._OnInnerPong(session, message);
        var rsp = message as InnerPongRsp;
        var config = G.GroupConfig.GetServerConfigByName(rsp.ServerName);
        if (config == null)
        {
            Logger.Warning("unknown server name");
            return;
        }
        if (config is GMServerConfig)
        {
            _TryConnectGateServer();
        }
        if (config is GateServerConfig)
        {
            _ConnectedGates += 1;
            if (_ConnectedGates == G.GroupConfig.GateServers.Count)
            {
                _OnAllGatesConnected();
            }
        }
    }

    private void _OnAllGatesConnected()
    {
        Logger.Info($"game is ready! {G.ServerConfig.Name}");
        var ntf = new ServerReadyNtf()
        {
            ServerName = G.ServerConfig.Name
        };
        G.GM.Send(ntf);
    }
    
    private int _ConnectedGates = 0;

}