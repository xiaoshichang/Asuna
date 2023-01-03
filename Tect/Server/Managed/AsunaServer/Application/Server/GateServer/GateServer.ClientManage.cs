using AsunaServer.Application;
using AsunaServer.Message;
using AsunaServer.Logger;
using AsunaServer.Network;

namespace AsunaServer.Application;

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
    
    private void _OnReceiveClientMessage(TcpSession session, object message)
    {
        if (!_HandleMessage(session, message))
        {
            Logger.Error($"message unhandled! {message.GetType()}");
        }
    }
    
}