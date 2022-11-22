using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Network;
using AsunaServer.Foundation.Timer;


namespace AsunaServer.Application.Server;


public abstract partial class ServerBase
{
    protected void _TryConnectGMSever()
    {
        if (IsGMServer())
        {
            Logger.Warning("gm try connect to self.");
            return;
        }

        uint delay = 1000;
        TimerMgr.AddTimer(delay, _DoConnectGMServer, null);
    }

    private void _DoConnectGMServer(object? param)
    {
        var config = _GroupConfig.GetGMConfig();
        InnerNetwork.ConnectTo(config.InternalIP, config.InternalPort, _ConnectToGMServer);
    }

    protected virtual void _ConnectToGMServer(TcpSession session)
    {
        var message = new InnerPingReq()
        {
            ServerName = _ServerConfig.Name
        };
        session.Send(message);
    }
    
}