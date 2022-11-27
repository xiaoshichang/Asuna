namespace AsunaServer.Application.Server.InnerMessage;

public class StubsDistributeNtf : INetworkMessage
{
    public Dictionary<string, string> StubsDistributeTable { get; set; } = new();
}

public class StubReadyNtf : INetworkMessage
{
    public string StubName { get; set; } = string.Empty;
}