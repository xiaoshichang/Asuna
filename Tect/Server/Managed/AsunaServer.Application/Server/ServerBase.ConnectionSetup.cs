using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Network;
using AsunaServer.Foundation.Timer;


namespace AsunaServer.Foundation.Server;


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
        Logger.Debug($"_DoConnectGMServer");
        var config = _GroupConfig.GetGMConfig();
        InnerNetwork.ConnectTo(config.InternalIP, config.InternalPort, _OnConnectToGMServer);
    }

    protected virtual void _OnConnectToGMServer(TcpSession session)
    {
        Logger.Debug("_OnConnectToGMServer");
    }
    
}