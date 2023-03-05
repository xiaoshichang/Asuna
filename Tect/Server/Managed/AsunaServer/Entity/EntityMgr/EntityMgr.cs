
using AsunaServer.Foundation.Debug;

namespace AsunaServer.Entity
{
    public static partial class EntityMgr
    {
        public static void AddEntity(Guid guid, ServerEntity entity)
        {
            if (_Entities.ContainsKey(guid))
            {
                ADebug.Error($"entity already exist! guid:{guid}");
                return;
            }
            _Entities.Add(guid, entity);
        }

        public static void RemoveEntity(Guid guid)
        {
            _Entities.Remove(guid);
        }
        
        private static readonly Dictionary<Guid, ServerEntity> _Entities = new();
    }
    
    
}

