using AsunaServer.Foundation.Network;
using AsunaServer.Foundation.Timer;

namespace AsunaServer.Application.Server;

public partial class GameServer : ServerBase
{
    protected void _TryConnectGMSever()
    {
        TimerMgr.AddTimer((uint)Random.Shared.Next(1000, 2000), _DoConnectGMServer, null);
    }

    private void _DoConnectGMServer(object? param)
    {
        var config = _GroupConfig.GetGMConfig();
        InnerNetwork.ConnectTo(config.InternalIP, config.InternalPort);
    }
    
    protected void _TryConnectGateServer()
    {
        TimerMgr.AddTimer((uint)Random.Shared.Next(2000, 3000), _DoConnectGateServer, null);
    }
        
    private void _DoConnectGateServer(object? param)
    {
        foreach (var gate in _GroupConfig.GateServers)
        {
            InnerNetwork.ConnectTo(gate.InternalIP, gate.InternalPort);
        }
    }
}