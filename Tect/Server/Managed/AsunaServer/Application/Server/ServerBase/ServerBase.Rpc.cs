using System.Reflection;
using AsunaServer.Debug;
using AsunaServer.Entity;
using AsunaServer.Message;
using AsunaServer.Network;

namespace AsunaServer.Application;

public abstract partial class ServerBase
{
    private ServerEntity? _FindCaller(RpcNtf rpc)
    {
        if (rpc.HasStubName)
        {
            return EntityMgr.GetStub(rpc.StubName);
        }
        else if (rpc.HasGuid)
        {
            throw new NotImplementedException();
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    private MethodInfo? _FindMethod(RpcNtf rpc)
    {
        var method = G.RPCTable.GetMethodByIndex(rpc.Method);
        if (method == null)
        {
            Logger.Error($"_OnRpcCall index {rpc.Method} not found.");
            return null;
        }
        return method;
    }

    private bool _CheckBeforeConvert(RpcNtf rpc)
    {
        if (rpc.Args.Count > 8)
        {
            Logger.Error("too much args");
            return false;
        }

        return true;
    }

    private bool _CheckBeforeInvoke(MethodInfo method, RpcNtf rpc)
    {
        var parameterCount = method.GetParameters().Length;
        if (parameterCount != rpc.ArgsCount)
        {
            Logger.Error($"_OnRpcCall method {method} arg count not match. {parameterCount} != {rpc.ArgsCount}");
            return false;
        }

        return true;
    }

    private bool _ConvertArgs(RpcNtf rpc, out object[] args)
    {
        args = new object[rpc.ArgsCount];
        for (var i = 0; i < rpc.ArgsCount; i++)
        {
            var str = rpc.Args[i].ToStringUtf8();
            var type = G.RPCTable.GetTypeByIndex(rpc.ArgsTypeIndex[i]);
            var obj = System.Text.Json.JsonSerializer.Deserialize(str, type);
            
            if (obj == null)
            {
                Logger.Error($"_OnRpcCall {i}-th arg is null, do not supported.");
                return false;
            }
            else
            {
                args[i] = obj;
            }
        }
        return true;
    }
    
    protected void _OnRpcCall(TcpSession session, object message)
    {
        if (message is not RpcNtf rpc)
        {
            Logger.Error($"unknown error.");
            return;
        }
        ServerEntity? caller = _FindCaller(rpc);
        if (caller == null)
        {
            return;
        }
        MethodInfo? method = _FindMethod(rpc);
        if (method == null)
        {
            return;
        }
        if (!_CheckBeforeConvert(rpc))
        {
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
        method.Invoke(caller, args);
    }
    
}