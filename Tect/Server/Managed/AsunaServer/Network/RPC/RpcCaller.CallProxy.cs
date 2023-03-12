using AsunaServer.Foundation.Debug;
using AsunaServer.Message;
using AsunaServer.Utils;
using Google.Protobuf;

namespace AsunaServer.Network;

public static partial class RpcCaller
{
    public static void CallProxy(AccountProxy proxy, string method, object[] args)
    {
        var rpc = new AccountRpc()
        {
            Guid = proxy.AccountID.ToProto(),
            Method = HashFunction.RpcToUint("Account", method),
            ArgsCount = args.Length
        };

        foreach (var arg in args)
        {
            RpcHelper.SerializeRpcArg(arg, out var data, out var index);
            rpc.ArgsTypeIndex.Add(index);
            rpc.Args.Add(ByteString.CopyFrom(data));
        }

        var session = Sessions.GetSessionByName(proxy.Gate);
        if (session == null)
        {
            ADebug.Error($"Session {proxy.Gate} not found.");
            return;
        }
        
        session.Send(rpc);
    }

    public static void CallProxy(AccountProxy proxy, string method)
    {
        CallProxy(proxy, method, new object[] { });
    }
    
    public static void CallProxy(AccountProxy proxy, string method, object arg1)
    {
        CallProxy(proxy, method, new object[] { arg1 });
    }
    
    public static void CallProxy(AccountProxy proxy, string method, object arg1, object arg2)
    {
        CallProxy(proxy, method, new object[] { arg1, arg2 });
    }
    
    public static void CallProxy(AccountProxy proxy, string method, object arg1, object arg2, object arg3)
    {
        CallProxy(proxy, method, new object[] { arg1, arg2, arg3 });
    }
    
    
}