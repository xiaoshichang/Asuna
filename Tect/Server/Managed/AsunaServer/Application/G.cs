using System.Reflection;
using System.Text.Json.Serialization;
using AsunaServer.Debug;
using AsunaServer.Entity;
using AsunaServer.Message;
using AsunaServer.Network;
using AsunaServer.Utils;
using Google.Protobuf;
using Google.Protobuf.Collections;

#pragma warning disable CS8618

namespace AsunaServer.Application
{
    public static class G
    {

        #region Config
        /// <summary>
        /// 服务器组配置
        /// </summary>
        public static ServerGroupConfig GroupConfig;
        
        /// <summary>
        /// 本服务器配置
        /// </summary>
        public static ServerConfigBase ServerConfig;
        #endregion
        
        # region Servers
        public static readonly Dictionary<string, TcpSession> ServerToSession = new();
        public static readonly Dictionary<TcpSession, string> SessionToServer = new();
        public static TcpSession? GM;
        public static readonly Dictionary<string, TcpSession> Games = new ();
        public static readonly Dictionary<string, TcpSession> Gates = new ();

        public static bool RegisterSession(string name, TcpSession session)
        {
            Logger.Assert(!ServerToSession.ContainsKey(name));    
            Logger.Assert(!SessionToServer.ContainsKey(session));
            ServerToSession[name] = session;
            SessionToServer[session] = name;

            var config = GroupConfig.GetServerConfigByName(name);
            if (config is GMServerConfig)
            {
                Logger.Assert(GM is null);
                GM = session;
            }
            else if (config is GameServerConfig)
            {
                Logger.Assert(!Games.ContainsKey(name));
                Games.Add(name, session);
            }
            else if (config is GateServerConfig)
            {
                Logger.Assert(!Gates.ContainsKey(name));
                Gates.Add(name, session);
            }
            else
            {
                throw new NotImplementedException();
            }
            return true;
        }

        public static bool UnRegisterSession(TcpSession session)
        {
            Logger.Assert(SessionToServer.ContainsKey(session));
            var name = SessionToServer[session];
            Logger.Assert(ServerToSession.ContainsKey(name));    
            
            var config = GroupConfig.GetServerConfigByName(name);
            if (config is GMServerConfig)
            {
                Logger.Assert(GM is not null);
                GM = null;
            }
            else if (config is GameServerConfig)
            {
                Logger.Assert(Games.ContainsKey(name));
                Games.Remove(name);
            }
            else if (config is GateServerConfig)
            {
                Logger.Assert(Gates.ContainsKey(name));
                Gates.Remove(name);
            }
            else
            {
                throw new NotImplementedException();
            }
            ServerToSession.Remove(name);
            SessionToServer.Remove(session);
            return true;
        }
        # endregion

        #region RPC
        /// <summary>
        /// 网络消息序列化器
        /// </summary>
        public static SerializerBase MessageSerializer = new ProtobufSerializer();

        /// <summary>
        /// RPC 索引表
        /// </summary>
        public static RpcTable RPCTable = new();
        
        #endregion
        
        #region Stub Table
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
                Logger.Error($"stub {stubName} not exist");
                return;
            }
            var method = stub.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method == null)
            {
                Logger.Error($"method {stubName}.{methodName} not exist");
                return;
            }
            if (!StubsDistributeTable.TryGetValue(stubName, out var server))
            {
                Logger.Error($"stub {stubName} not exist in StubsDistributeTable");
                return;
            }
            if (!ServerToSession.TryGetValue(server, out var session))
            {
                Logger.Error($"session {server} not exist");
                return;
            }
            var rpc = new RpcNtf
            {
                StubName = stubName,
                Method = HashFunction.MethodToUint(method),
                ArgsCount = (uint)args.Length,
            };
            foreach (var arg in args)
            {
                var index = HashFunction.StringToUint(arg.GetType().Name);
                rpc.ArgsTypeIndex.Add(index);
                var str = System.Text.Json.JsonSerializer.Serialize(arg);
                var bin = System.Text.Encoding.UTF8.GetBytes(str);
                rpc.Args.Add(ByteString.CopyFrom(bin));
            }
            session.Send(rpc);
        }
        #endregion

    }
}

