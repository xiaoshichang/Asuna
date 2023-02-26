using AsunaServer.Application;

namespace AsunaServer.Application;

public abstract partial class ServerBase
{
    public bool IsGMServer()
    {
        return G.ServerConfig is GMServerConfig;
    }

    public bool IsGameServer()
    {
        return G.ServerConfig is GameServerConfig;
    }

    public bool IsGateServer()
    {
        return G.ServerConfig is GateServerConfig;
    }
}