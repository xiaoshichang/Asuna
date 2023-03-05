﻿using AsunaServer.Application;
using AsunaServer.Message;
using AsunaServer.Foundation.Debug;
using AsunaServer.Network;
using AsunaServer.Timer;

namespace AsunaServer.Application;

#pragma warning disable CS8602

public partial class GateServer : ServerBase
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
    
    protected override void _OnInnerPing(TcpSession session, object message)
    {
        base._OnInnerPing(session, message);
        var req = message as InnerPingReq;
        var config = G.GroupConfig.GetServerConfigByName(req.ServerName);
        if (config is GameServerConfig)
        {
            _ConnectedGames += 1;
            _CheckServerReady();
        }
    }

    protected override void _OnInnerPong(TcpSession session, object message)
    {
        base._OnInnerPong(session, message);
        var rsp = message as InnerPongRsp;
        var config = G.GroupConfig.GetServerConfigByName(rsp.ServerName);
        if (config is GMServerConfig)
        {
            _CheckServerReady();
        }
    }

    /// <summary>
    /// 对于Gate而言，Ready条件为
    ///     -   所有Game连接成功
    /// </summary>
    private void _CheckServerReady()
    {
        if (_ConnectedGames < G.GroupConfig.GameServers.Count)
        {
            return;
        }
        
        ADebug.Info($"gate is ready! {G.ServerConfig.Name}");
        var ntf = new ServerReadyNtf()
        {
            ServerName = G.ServerConfig.Name
        };
        Sessions.GM.Send(ntf);
    }

    private int _ConnectedGames = 0;

}