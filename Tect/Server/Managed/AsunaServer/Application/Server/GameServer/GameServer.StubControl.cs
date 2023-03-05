using AsunaServer.Message;
using AsunaServer.Entity;
using AsunaServer.Foundation.Debug;
using AsunaServer.Network;

#pragma warning disable CS8602

namespace AsunaServer.Application;

public partial class GameServer : ServerBase
{
    
    private void _OnStubsDistributeNtf(TcpSession session, object message)
    {
        var ntf = message as StubsDistributeNtf;
        RpcCaller.StubsDistributeTable = ntf.StubsDistributeTable;
        foreach (var item in RpcCaller.StubsDistributeTable)
        {
            var stubType = EntityMgr.GetStubTypeByName(item.Key);
            if (stubType == null)
            {
                ADebug.Error("unknown stub name");
                continue;
            }
            if (item.Value == G.ServerConfig.Name)
            {
                try
                {
                    var stub = Activator.CreateInstance(stubType) as ServerStubEntity;
                    if (stub == null)
                    {
                        ADebug.Error("not a server stub!");
                        continue;
                    }
                    stub.Init();
                    EntityMgr.RegisterStub(item.Key, stub);
                }
                catch (Exception e)
                {
                    ADebug.Error($"startup stub with exception: {e.Message}");
                }
            }
        }
    }
    
}