using AsunaServer.Application.Server.InnerMessage;
using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Network;

namespace AsunaServer.Application.Server;

public partial class GMServer  : ServerBase
{
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
        Logger.Info("_OnAllServerReady");
    }

    private int _ReadyServerCount = 0;
}