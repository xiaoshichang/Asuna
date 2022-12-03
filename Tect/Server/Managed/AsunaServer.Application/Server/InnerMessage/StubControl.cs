using AsunaServer.Foundation.Network.Message;

namespace AsunaServer.Application.Server.InnerMessage;

public class StubsDistributeNtf : NetworkMessage
{
    public Dictionary<string, string> StubsDistributeTable { get; set; } = new();
}

public class StubReadyNtf : NetworkMessage
{
    public string StubName { get; set; } = string.Empty;
}