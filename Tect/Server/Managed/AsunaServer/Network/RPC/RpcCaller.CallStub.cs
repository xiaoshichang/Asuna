using System.Reflection;
using AsunaServer.Application;
using AsunaServer.Foundation.Debug;
using AsunaServer.Entity;
using AsunaServer.Message;
using AsunaServer.Utils;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace AsunaServer.Network
{
    public static partial class RpcCaller
    {
        /// <summary>
        /// Stub分布表
        /// </summary>
        public static MapField<string, string> StubsDistributeTable = new();

        /// <summary>
        /// 对 ServerStub 发起 RPC 调用
        /// </summary>
        /// <param name="stubName"></param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        public static void CallStub(string stubName, string methodName, object[] args)
        {
            var stub = EntityMgr.GetStubTypeByName(stubName);
            if (stub == null)
            {
                ADebug.Error($"stub {stubName} not exist");
                return;
            }
            if (!StubsDistributeTable.TryGetValue(stubName, out var server))
            {
                ADebug.Error($"stub {stubName} not exist in StubsDistributeTable");
                return;
            }
            if (!Sessions.ServerToSession.TryGetValue(server, out var session))
            {
                ADebug.Error($"session {server} not exist");
                return;
            }
            var rpc = new StubRpc()
            {
                StubName = stubName,
                Method = HashFunction.RpcToUint(stubName, methodName),
                ArgsCount = (uint)args.Length,
            };
            foreach (var arg in args)
            {
                RpcHelper.SerializeRpcArg(arg, out var data, out var index);
                rpc.ArgsTypeIndex.Add(index);
                rpc.Args.Add(ByteString.CopyFrom(data));
            }
            session.Send(rpc);
        }

        public static void CallStub(string stubName, string methodName, object arg1)
        {
            CallStub(stubName, methodName, new[] { arg1 });
        }
        
        public static void CallStub(string stubName, string methodName, object arg1, object arg2)
        {
            CallStub(stubName, methodName, new[] { arg1, arg2 });
        }
        
        public static void CallStub(string stubName, string methodName, object arg1, object arg2, object arg3)
        {
            CallStub(stubName, methodName, new[] { arg1, arg2, arg3 });
        }
        
        public static void CallStub(string stubName, string methodName, object arg1, object arg2, object arg3, object arg4)
        {
            CallStub(stubName, methodName, new[] { arg1, arg2, arg3, arg4 });
        }
        
        
    }
}

