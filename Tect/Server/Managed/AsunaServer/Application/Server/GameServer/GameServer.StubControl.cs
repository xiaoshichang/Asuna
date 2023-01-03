using AsunaServer.Message;
using AsunaServer.Entity;
using AsunaServer.Debug;
using AsunaServer.Network;
using Google.Protobuf.Collections;

#pragma warning disable CS8602

namespace AsunaServer.Application;

public partial class GameServer : ServerBase
{
    
    private void _OnStubsDistributeNtf(TcpSession session, StubsDistributeNtf ntf)
    {
        _StubsDistributeTable = ntf.StubsDistributeTable;
        foreach (var item in _StubsDistributeTable)
        {
            var stubType = EntityMgr.GetStubByName(item.Key);
            if (stubType == null)
            {
                Logger.Error("unknown stub name");
                continue;
            }
            if (item.Value == _ServerConfig.Name)
            {
                try
                {
                    var stub = Activator.CreateInstance(stubType) as ServerStubEntity;
                    if (stub == null)
                    {
                        Logger.Error("not a server stub!");
                        continue;
                    }
                    stub.SetCallGMDelegate(_CallGM);
                    stub.Init();
                }
                catch (Exception e)
                {
                    Logger.Error($"startup stub with exception: {e.Message}");
                }
            }
        }
    }

    private MapField<string, string> _StubsDistributeTable = new();
}