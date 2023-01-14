using System;
using System.Collections.Generic;
using System.Linq;
using Asuna.Utils;

namespace Asuna.Entity
{
    public partial class EntityManager
    {
        
        public AvatarEntity CreateAvatarEntity()
        {
            var avatar = new AvatarEntity();
            avatar.Init();
            _Entities[avatar.Guid] = avatar;
            _AvatarEntities[avatar.Guid] = avatar;
            return avatar;
        }

        public void DestroyAvatar(AvatarEntity avatar)
        {
            ADebug.Assert(_Entities.ContainsKey(avatar.Guid));
            ADebug.Assert(_AvatarEntities.ContainsKey(avatar.Guid));
            
            avatar.Destroy();
            _AvatarEntities.Remove(avatar.Guid);
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