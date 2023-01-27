using Asuna.Application;
using Asuna.Input;
using Asuna.Utils;

namespace Asuna.Entity
{
    public class PlayerInputComponentInitParam
    {
        public bool PlayerInputCandidate = false;
    }


    public class PlayerInputComponent : Component
    {
        public override void Init(Entity owner, object param)
        {
            _Owner = owner as AvatarEntity;
            _PlayerInputComponentInitParam = param as PlayerInputComponentInitParam;
            ADebug.Assert(_Owner != null);
            ADebug.Assert(_PlayerInputComponentInitParam != null);
        }

        public override void Release()
        {
            if (_CurrentMapping != null)
            {
                UnBindPlayerInput();
            }
            _Owner = null;
        }

        public void Update(float dt)
        {
            
        }

        public void BindPlayerInput(PlayerType playerType)
        {
            ADebug.Assert(_CurrentMapping == null);
            _CurrentMapping = G.PlayerInputManager.GetMapping(playerType);
        }

        public void UnBindPlayerInput()
        {
            ADebug.Assert(_CurrentMapping != null);
            _CurrentMapping = null;
        }

        private PlayerInputComponentInitParam _PlayerInputComponentInitParam;
        private PlayerInputMapping _CurrentMapping;
        private AvatarEntity _Owner;
    }
}