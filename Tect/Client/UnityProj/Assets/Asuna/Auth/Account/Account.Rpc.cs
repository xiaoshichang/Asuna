using System.Reflection;
using Asuna.Application;
using Asuna.Foundation.Debug;
using Asuna.Network;
using Asuna.Utils;
using AsunaShared.Message;
using Google.Protobuf;
using Newtonsoft.Json;

namespace Asuna.Auth
{
    public partial class Account
    {
        public void CallServer(string methodName, object[] args)
        {
            var rpc = new AccountRpc()
            {
                Guid = Guid.ToProto(),
                Method = HashFunction.RpcToUint("Account", methodName),
                ArgsCount = args.Length,
            };
            
            foreach (var arg in args)
            {
                var index = HashFunction.StringToUint(arg.GetType().Name);
                rpc.ArgsTypeIndex.Add(index);
                var str = JsonConvert.SerializeObject(arg);
                var bin = System.Text.Encoding.UTF8.GetBytes(str);
                rpc.Args.Add(ByteString.CopyFrom(bin));
            }
            G.NetworkManager.Send(rpc);
        }
        
        private static bool _ConvertArgs(AccountRpc rpc, out object[] args)
        {
            args = new object[rpc.ArgsCount];
            for (var i = 0; i < rpc.ArgsCount; i++)
            {
                var str = rpc.Args[i].ToStringUtf8();
                var type = RpcTable.GetTypeByIndex(rpc.ArgsTypeIndex[i]);
                var obj = JsonConvert.DeserializeObject(str, type);
            
                if (obj == null)
                {
                    ADebug.Error($"_ConvertArgs {i}-th arg is null, do not supported.");
                    return false;
                }
                else
                {
                    args[i] = obj;
                }
            }
            return true;
        }
        
        private static bool _CheckBeforeInvoke(MethodInfo method, AccountRpc rpc)
        {
            var parameterCount = method.GetParameters().Length;
            if (parameterCount != rpc.ArgsCount)
            {
                ADebug.Error($"_OnRpcCall method {method} arg count not match. {parameterCount} != {rpc.ArgsCount}");
                return false;
            }
            return true;
        }

        public static void _OnAccountRpc(object ntf)
        {
            var rpc = ntf as AccountRpc;
            if (rpc == null)
            {
                ADebug.Error("_OnAccountRpc unknown error");
                return;
            }
            if (G.Account == null)
            {
                ADebug.Error("_OnAccountRpc account not auth");
                return;
            }
            var method = RpcTable.GetMethodByIndex(rpc.Method);
            if (method == null)
            {
                ADebug.Error($"method not found {rpc.Method}");
                return;
            }
            if (!_ConvertArgs(rpc, out var args))
            {
                return;
            }
            if (!_CheckBeforeInvoke(method, rpc))
            {
                return;
            }
            method.Invoke(G.Account, args);
        }
        
    }
}