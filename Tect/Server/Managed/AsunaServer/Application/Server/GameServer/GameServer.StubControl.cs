using AsunaServer.Message;
using AsunaServer.Entity;
using AsunaServer.Debug;
using AsunaServer.Network;

#pragma warning disable CS8602

namespace AsunaServer.Application;

public partial class GameServer : ServerBase
{
    
    private void _OnStubsDistributeNtf(TcpSession session, object message)
    {
        var ntf = message as StubsDistributeNtf;
        G.StubsDistributeTable = ntf.StubsDistributeTable;
        foreach (var item in G.StubsDistributeTable)
        {
            var stubType = EntityMgr.GetStubTypeByName(item.Key);
            if (stubType == null)
            {
                Logger.Error("unknown stub name");
                continue;
            }
            if (item.Value == G.ServerConfig.Name)
            {
                try
                {
                    var stub = Activator.CreateInstance(stubType) as ServerStubEntity;
                    if (stub == null)
                    {
                        Logger.Error("not a server stub!");
                        continue;
                    }
                    stub.Init();
                    EntityMgr.RegisterStub(item.Key, stub);
                }
                catch (Exception e)
                {
                    Logger.Error($"startup stub with exception: {e.Message}");
                }
            }
        }
    }
    
}