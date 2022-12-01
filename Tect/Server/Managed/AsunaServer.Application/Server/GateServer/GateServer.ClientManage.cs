﻿using AsunaServer.Application.Config;
using AsunaServer.Application.Server.InnerMessage;
using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Network;

namespace AsunaServer.Application.Server;

public partial class GateServer : ServerBase
{
    private void _OnOpenGateNtf(TcpSession session, OpenGateNtf message)
    {
        var gateConfig = _ServerConfig as GateServerConfig;
        if (gateConfig == null)
        {
            Logger.Error("unknown config type!");
            return;
        }

        OuterNetwork.Init(gateConfig.OuterIp, gateConfig.OuterPort, _OnAccepClientConnection, null, _OnReceiveClientMessage);
        Logger.Info($"open gate at {gateConfig.OuterIp} {gateConfig.OuterPort}");
    }

    private void _OnAccepClientConnection(TcpSession session)
    {
        // todo: remove client if timeout without login
        Logger.Debug($"on client connected! {session.GetConnectionID()}");
    }
    
    private void _OnReceiveClientMessage(TcpSession session, object message, Type type)
    {
        if (!_HandleMessage(session, message, type))
        {
            Logger.Error($"message unhandled! {type}");
        }
    }
    
}