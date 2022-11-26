namespace AsunaServer.Application.Server.InnerMessage;

public class InnerPingReq : INetworkMessage
{
    public string ServerName { get; set; } = string.Empty;
}

public class InnerPongRsp : INetworkMessage
{
    public string ServerName { get; set; } = string.Empty;
}

public class ServerReadyNtf : INetworkMessage
{
    public string ServerName { get; set; } = string.Empty;
}