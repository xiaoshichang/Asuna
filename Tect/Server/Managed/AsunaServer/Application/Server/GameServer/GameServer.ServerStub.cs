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
    
    protected void _OnStubRpc(TcpSession session, object message)
    {
        if (message is not StubRpc rpc)
        {
            ADebug.Error($"unknown error.");
            return;
        }
        var stub = EntityMgr.GetStub(rpc.StubName);
        if (stub == null)
        {
            ADebug.Warning($"Stub not found {rpc.StubName}");
            return;
        }
        var method = RpcTable.GetMethodByIndex(rpc.Method);
        if (method == null)
        {
            ADebug.Warning($"method not found {rpc.Method}");
            return;
        }
        var ok = _ConvertArgs(rpc, out var args);
        if (!ok)
        {
            return;
        }
        if (!_CheckBeforeInvoke(method, rpc))
        {
            return;
        }
        method.Invoke(stub, args);
    }
    
}