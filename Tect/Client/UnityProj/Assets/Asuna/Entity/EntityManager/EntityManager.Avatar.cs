using System;
using System.Collections.Generic;
using System.Linq;
using Asuna.Utils;

namespace Asuna.Entity
{
    public partial class EntityManager
    {
        
        public SpaceEntity CreateAvatarEntity()
        {
            var space = new SpaceEntity();
            space.Init();
            _Entities[space.Guid] = space;
            _SpaceEntities[space.Guid] = space;
            return space;
        }

        public void DestroyAvatar(AvatarEntity avatar)
        {
            ADebug.Assert(_Entities.ContainsKey(avatar.Guid));
            ADebug.Assert(_SpaceEntities.ContainsKey(avatar.Guid));
            
            avatar.Destroy();
            _SpaceEntities.Remove(avatar.Guid);
            _Entities.Remove(avatar.Guid);

        }

        private void DestroyAllAvatars()
        {
            while (_AvatarEntities.Count > 0)
            {
                var avatar = _AvatarEntities.First().Value;
                DestroyAvatar(avatar);
            }
        }
        
        private readonly Dictionary<Guid, AvatarEntity> _AvatarEntities = new Dictionary<Guid, AvatarEntity>();
    }
}