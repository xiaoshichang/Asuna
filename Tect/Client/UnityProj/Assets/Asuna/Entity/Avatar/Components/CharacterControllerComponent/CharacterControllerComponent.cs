using Asuna.Foundation.Debug;

namespace Asuna.Entity
{
    public enum ControllerType
    {
        None,
        NativeAnimator,
        SimpleFSM
    }

    /// <summary>
    /// 当前的角色控制输入来源
    /// </summary>
    public enum ControllerInputSource
    {
        /// <summary>
        /// 无来源
        /// </summary>
        None,
        
        /// <summary>
        /// 由玩家控制
        /// </summary>
        PlayerInput,
        
        /// <summary>
        /// 由AI驱动
        /// </summary>
        AI
    }
    
    public class CharacterControllerComponentInitParam
    {
        public ControllerType ControllerType = ControllerType.None;
    }
    
    
    public abstract class CharacterControllerComponent : Component
    {
        public override void Init(Entity owner, object param)
        {
            _Owner = owner as AvatarEntity;
            _InitParam = param as CharacterControllerComponentInitParam;
            ADebug.Assert(_Owner != null);
            ADebug.Assert(_InitParam != null);
        }

        public override void Release()
        {
            _Owner = null;
            _InitParam = null;
        }

        public void SetInputSource(ControllerInputSource source)
        {
            _InputSource = source;
        }

        public abstract void AfterModelLoaded();
        public abstract void Update(float dt);
        public abstract void LateUpdate(float dt);

        protected AvatarEntity _Owner;
        protected CharacterControllerComponentInitParam _InitParam;
        protected ControllerInputSource _InputSource = ControllerInputSource.None;

    }
}