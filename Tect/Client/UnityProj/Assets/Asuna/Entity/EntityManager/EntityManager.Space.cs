using System;
using System.Collections.Generic;
using Asuna.Utils;
using UnityEngine;

namespace Asuna.Entity
{
    public partial class EntityManager
    {
        public SpaceEntity CreateSpaceEntity()
        {
            var space = new SpaceEntity();
            _Entities[space.Guid] = space;
            _SpaceEntities[space.Guid] = space;
            return space;
        }

        public void DestroySpace(SpaceEntity space)
        {
            ADebug.Assert(_Entities.ContainsKey(space.Guid));
            ADebug.Assert(_SpaceEntities.ContainsKey(space.Guid));
            
            space.Release();
            _SpaceEntities.Remove(space.Guid);
            _Entities.Remove(space.Guid);

        }

        private readonly Dictionary<Guid, SpaceEntity> _SpaceEntities = new Dictionary<Guid, SpaceEntity>();
    }
}