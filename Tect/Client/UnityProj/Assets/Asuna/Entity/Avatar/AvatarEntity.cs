using System.Collections;
using Asuna.Input;

namespace Asuna.Entity
{
    /// <summary>
    /// Avatar的初始化数据
    /// </summary>
    public class AvatarInitParam
    {
        public readonly ModelComponentInitParam ModelComponentInitParam = new ();
        public readonly CharacterControllerComponentInitParam CharacterControllerComponentInitParam = new ();
    }
    
    
    public class AvatarEntity : Entity
    {
        protected override string GetGameObjectName()
        {
            return $"Avatar-{Guid:B}";
        }

        private void _InitModelComponent()
        {
            ModelComponent.Init(this, AvatarInitParam.ModelComponentInitParam);
        }

        private void _InitAnimatorComponent()
        {
            var initParam = AvatarInitParam.CharacterControllerComponentInitParam;
            var mode = AvatarInitParam.CharacterControllerComponentInitParam.ControllerType;

            if (mode == ControllerType.None)
            {
            }
            else if (mode == ControllerType.NativeAnimator)
            {
                CharacterControllerComponent = new AnimatorCharacterControllerComponent();
                CharacterControllerComponent.Init(this, initParam);
                CharacterControllerComponent.SetInputSource(ControllerInputSource.PlayerInput);
            }
            else if (mode == ControllerType.SimpleFSM)
            {
                CharacterControllerComponent = new SimpleFSMCharacterControllerComponent();
                CharacterControllerComponent.Init(this, initParam);
                CharacterControllerComponent.SetInputSource(ControllerInputSource.PlayerInput);
            }
        }
        
        /// <summary>
        /// 初始化 - 数据部分
        /// </summary>
        public override void Init(object param)
        {
            base.Init(null);
            AvatarInitParam = param as AvatarInitParam;
            _InitModelComponent();
            _InitAnimatorComponent();
           
        }

        public override void Destroy()
        {
            CharacterControllerComponent?.Release();
            ModelComponent.Release();
            base.Destroy();
        }

        public override void Update(float dt)
        {
            CharacterControllerComponent?.Update(dt);
        }

        public override void LateUpdate(float dt)
        {
            CharacterControllerComponent?.LateUpdate(dt);
        }

        /// <summary>
        /// 异步加载 - 资源部分
        /// </summary>
        public IEnumerator LoadAsync()
        {
            yield return ModelComponent.LoadModelAsync();
            _AfterModelLoaded();
        }

        protected virtual void _AfterModelLoaded()
        {
            CharacterControllerComponent?.AfterModelLoaded();
        }


        public AvatarInitParam AvatarInitParam;
        public readonly ModelComponent ModelComponent = new();
        public CharacterControllerComponent CharacterControllerComponent;


    }
}