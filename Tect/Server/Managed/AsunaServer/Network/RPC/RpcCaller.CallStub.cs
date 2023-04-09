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
        /// 对 ServerStub 发起直接调用
        /// </summary>
        public static void CallStubLocal(string stubName, string methodName, object[] args)
        {
            var stub = EntityMgr.GetStub(stubName);
            if (stub == null)
            {
                ADebug.Error($"CallStubLocal - stub {stubName} not found!");
                return;
            }

            var method = stub.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method == null)
            {
                ADebug.Error("CallStubLocal - method not found");
                return;
            }

            method.Invoke(stub, args);
        }
        
        /// <summary>
        /// 从Game上对 ServerStub 发起 RPC 调用
        /// </summary>
        public static void CallStubFromGame(string server, string stubName, string methodName, object[] args)
        {
            var stub = EntityMgr.GetStubTypeByName(stubName);
            if (stub == null)
            {
                ADebug.Error($"CallStubRemote - stub {stubName} not exist");
                return;
            }
            var rpc = new StubRpc()
            {
                StubName = stubName,
                Method = HashUtils.RpcToUint(stubName, methodName),
                ArgsCount = (uint)args.Length,
                Server = server
            };
            foreach (var arg in args)
            {
                RpcHelper.SerializeRpcArg(arg, out var data, out var index);
                rpc.ArgsTypeIndex.Add(index);
                rpc.Args.Add(ByteString.CopyFrom(data));
            }

            var gate = RandomUtils.RandomGetItem(Sessions.Gates.Values.ToArray());
            gate.Send(rpc);
        }

        /// <summary>
        /// 从Gate上对 ServerStub 发起 RPC 调用
        /// </summary>
        public static void CallStubFromGate(string server, string stubName, string methodName, object[] args)
        {
            var stub = EntityMgr.GetStubTypeByName(stubName);
            if (stub == null)
            {
                ADebug.Error($"CallStubRemote - stub {stubName} not exist");
                return;
            }
            var rpc = new StubRpc()
            {
                StubName = stubName,
                Method = HashUtils.RpcToUint(stubName, methodName),
                ArgsCount = (uint)args.Length,
            };
            foreach (var arg in args)
            {
                RpcHelper.SerializeRpcArg(arg, out var data, out var index);
                rpc.ArgsTypeIndex.Add(index);
                rpc.Args.Add(ByteString.CopyFrom(data));
            }

            var game = Sessions.GetSessionByName(server);
            if (game == null)
            {
                ADebug.Error($"CallStubFromGate - {server} not found");
                return;
            }
            game.Send(rpc);
        }
        

        public static void CallStub(string stubName, string methodName, object[] args)
        {
            if (!StubsDistributeTable.TryGetValue(stubName, out var server))
            {
                ADebug.Error($"CallStub - stub {stubName} not exist in StubsDistributeTable");
                return;
            }

            if (server == G.ServerConfig.Name)
            {
                CallStubLocal(stubName, methodName, args);
            }
            else
            {
                if (G.ServerConfig is GateServerConfig)
                {
                    CallStubFromGate(server, stubName, methodName, args);
                }
                else if (G.ServerConfig is GameServerConfig)
                {
                    CallStubFromGame(server, stubName, methodName, args);
                }
                else
                {
                    ADebug.Error("CallStub - unknown error");
                }
            }
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

