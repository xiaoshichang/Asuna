using System.Reflection;
using AsunaServer.Foundation.Debug;
using AsunaServer.Entity;
using AsunaServer.Message;
using AsunaServer.Network;

namespace AsunaServer.Application;

public abstract partial class ServerBase
{

    private bool _CheckBeforeInvoke(MethodInfo method, StubRpc rpc)
    {
        var parameterCount = method.GetParameters().Length;
        if (parameterCount != rpc.ArgsCount)
        {
            ADebug.Error($"_OnRpcCall method {method} arg count not match. {parameterCount} != {rpc.ArgsCount}");
            return false;
        }

        return true;
    }

    private bool _ConvertArgs(StubRpc rpc, out object[] args)
    {
        args = new object[rpc.ArgsCount];
        for (var i = 0; i < rpc.ArgsCount; i++)
        {
            var str = rpc.Args[i].ToStringUtf8();
            var type = RpcTable.GetTypeByIndex(rpc.ArgsTypeIndex[i]);
            var obj = System.Text.Json.JsonSerializer.Deserialize(str, type);
            
            if (obj == null)
            {
                ADebug.Error($"_OnRpcCall {i}-th arg is null, do not supported.");
                return false;
            }
            else
            {
                args[i] = obj;
            }
        }
        return true;
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