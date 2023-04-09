using System.Reflection;
using AsunaServer.Foundation.Debug;
using AsunaServer.Entity;
using AsunaServer.Message;
using AsunaServer.Network;

namespace AsunaServer.Application;

public abstract partial class ServerBase
{

    protected bool _CheckBeforeInvoke(MethodInfo method, StubRpc rpc)
    {
        var parameterCount = method.GetParameters().Length;
        if (parameterCount != rpc.ArgsCount)
        {
            ADebug.Error($"_OnRpcCall method {method} arg count not match. {parameterCount} != {rpc.ArgsCount}");
            return false;
        }

        return true;
    }

    protected bool _ConvertArgs(StubRpc rpc, out object[] args)
    {
        args = new object[rpc.ArgsCount];
        for (var i = 0; i < rpc.ArgsCount; i++)
        {
            var obj = RpcHelper.DeserializeRpcArg(rpc.Args[i], rpc.ArgsTypeIndex[i]);
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

}