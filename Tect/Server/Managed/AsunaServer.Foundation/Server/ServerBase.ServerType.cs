using AsunaServer.Foundation.Config;

namespace AsunaServer.Foundation.Server;

public abstract partial class ServerBase
{
    public bool IsGMServer()
    {
        return _ServerConfig is GMServerConfig;
    }

    public bool IsGameServer()
    {
        return _ServerConfig is GameServerConfig;
    }

    public bool IsGateServer()
    {
        return _ServerConfig is GateServerConfig;
    }
}