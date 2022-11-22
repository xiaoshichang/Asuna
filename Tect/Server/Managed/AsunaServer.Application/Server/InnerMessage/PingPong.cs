using AsunaServer.Foundation.Network.Message.Indexer;

[NetworkMessage]
public class InnerPingReq
{
    public string ServerName { get; set; } = string.Empty;
}

[NetworkMessage]
public class InnerPongRsp
{
    public string ServerName { get; set; } = string.Empty;
}