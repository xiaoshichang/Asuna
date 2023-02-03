using System.Collections.Generic;
using Asuna.Entity;
using Asuna.Input;
using Asuna.Interface;
using Asuna.Utils;

namespace Asuna.Gameplay
{
    public class InputMappingSystem : ISystem
    {
        public void Init(object param)
        {
            
        }

        public void Release()
        {
        }

        public void BindPlayerInputToEntity(PlayerType player, AvatarEntity entity)
        {
            ADebug.Assert(entity != null, "entity is null");
            ADebug.Assert(!_Mapping.ContainsKey(player), "already bind");
            
            _Mapping[player] = entity;
            entity.OnBindToPlayerInput(player);
        }

        public void UnBindPlayerInputByPlayerType(PlayerType player)
        {
            ADebug.Assert(_Mapping.ContainsKey(player), $"{player} not binding");
            _Mapping[player].OnUnbindFromPlayerInput();
            _Mapping.Remove(player);
        }

        private readonly Dictionary<PlayerType, AvatarEntity> _Mapping = new Dictionary<PlayerType, AvatarEntity>();
    }
}