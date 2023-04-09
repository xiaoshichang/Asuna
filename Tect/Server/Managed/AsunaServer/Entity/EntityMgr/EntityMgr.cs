
using AsunaServer.Foundation.Debug;
using AsunaServer.Message;
using AsunaServer.Network;
using AsunaServer.Utils;
using Google.Protobuf;

namespace AsunaServer.Entity
{
    public static partial class EntityMgr
    {
        
        public static ServerEntity? CreateEntityLocal(Type entityType, object[] args)
        {
            try
            {
                var entity = Activator.CreateInstance(entityType, args) as ServerEntity;
                if (entity == null)
                {
                    ADebug.Error("create entity fail");
                    return null;
                }
                if (_Entities.ContainsKey(entity.Guid))
                {
                    ADebug.Error("already exist");
                    return null;
                }
                _Entities[entity.Guid] = entity;
                return entity;
            }
            catch (Exception e)
            {
                ADebug.Info(e.Message);
                return null;
            }
        }
        
        public static void CreateEntityRemote(Type entityType, object[] args)
        {
            var gate = RandomUtils.RandomGetItem(Sessions.Gates.Values.ToArray());
            CreateEntityRemote(entityType, args, gate);
        }

        public static void CreateEntityRemote(Type entityType, object arg1)
        {
            var args = new [] { arg1 };
            CreateEntityRemote(entityType, args);
        }
        
        public static void CreateEntityRemote(Type entityType, object arg1, object arg2)
        {
            var args = new [] { arg1, arg2 };
            CreateEntityRemote(entityType, args);
        }
        
        public static void CreateEntityRemote(Type entityType, object arg1, object arg2, object arg3)
        {
            var args = new [] { arg1, arg2, arg3 };
            CreateEntityRemote(entityType, args);
        }

        public static void CreateEntityRemote(Type entityType, object[] args, TcpSession gate)
        {
            var ntf = new CreateServerEntityNtf()
            {
                TypeIndex = HashUtils.ClassToUint(entityType),
                ArgsCount = args.Length
            };
            
            foreach (var arg in args)
            {
                RpcHelper.SerializeRpcArg(arg, out var data, out var index);
                ntf.ArgsTypeIndex.Add(index);
                ntf.Args.Add(ByteString.CopyFrom(data));
            }
            gate.Send(ntf);
        }

        private static readonly Dictionary<Guid, ServerEntity> _Entities = new();
    }
    
    
}

