using System;
using System.Collections.Generic;
using System.Linq;
using Asuna.Utils;
using UnityEngine;

namespace Asuna.Entity
{
    public partial class EntityManager
    {
        public SpaceEntity CreateSpaceEntity()
        {
            var space = new SpaceEntity();
            space.Init(null);
            _Entities[space.Guid] = space;
            _SpaceEntities[space.Guid] = space;
            return space;
        }

        public void DestroySpace(SpaceEntity space)
        {
            ADebug.Assert(_Entities.ContainsKey(space.Guid));
            ADebug.Assert(_SpaceEntities.ContainsKey(space.Guid));
            
            space.Destroy();
            _SpaceEntities.Remove(space.Guid);
            _Entities.Remove(space.Guid);
        }

        private void DestroyAllSpace()
        {
            while (_SpaceEntities.Count > 0)
            {
                var space = _SpaceEntities.First().Value;
                DestroySpace(space);
            }
        }

        private readonly Dictionary<Guid, SpaceEntity> _SpaceEntities = new Dictionary<Guid, SpaceEntity>();
    }
}