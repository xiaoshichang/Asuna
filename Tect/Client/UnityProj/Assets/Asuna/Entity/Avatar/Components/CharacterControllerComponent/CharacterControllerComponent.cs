using Asuna.Utils;

namespace Asuna.Entity
{
    public enum ControllerMode
    {
        None,
        NativeAnimator,
        SimpleFSM
    }
    
    public class CharacterControllerComponentInitParam
    {
        public ControllerMode ControllerMode = ControllerMode.None;
    }
    
    
    public abstract class CharacterControllerComponent : Component
    {
        public override void Init(Entity owner, object param)
        {
            _Owner = owner as AvatarEntity;
            _InitParam = param as CharacterControllerComponentInitParam;
            ADebug.Assert(_Owner != null);
        }

        public override void Release()
        {
            _Owner = null;
        }

        public abstract void AfterModelLoaded();
        public abstract void LateUpdate(float dt);
        
        protected AvatarEntity _Owner;
        protected CharacterControllerComponentInitParam _InitParam;

    }
}