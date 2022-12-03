using AsunaServer.Foundation.Network.Message;


namespace AsunaServer.Application.Server.InnerMessage;
public class InnerPingReq : NetworkMessage
{
    public string ServerName { get; set; } = string.Empty;
}

public class InnerPongRsp : NetworkMessage
{
    public string ServerName { get; set; } = string.Empty;
}

public class ServerReadyNtf : NetworkMessage
{
    public string ServerName { get; set; } = string.Empty;
}