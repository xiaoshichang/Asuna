
using AsunaServer.Foundation.Debug;

namespace AsunaServer.Entity
{
    public static partial class EntityMgr
    {
        public static void CreateEntityLocal(Type entityType)
        {
            
        }
        
        public static void CreateEntityRemote(Type entityType)
        {
            
        }
        
        public static void CreateEntityRemote(Type entityType, string game)
        {
            
        }
        
        private static readonly Dictionary<Guid, ServerEntity> _Entities = new();
    }
    
    
}

