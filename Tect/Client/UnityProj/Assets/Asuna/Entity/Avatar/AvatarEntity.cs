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
        public readonly PlayerInputComponentInitParam PlayerInputComponentInitParam = new ();
    }
    
    
    public class AvatarEntity : Entity
    {
        protected override string GetGameObjectName()
        {
            return $"Avatar-{Guid:B}";
        }

        private void _InitModelComponent()
        {
            ModelComponent.Init(this, AvatarInitParam);
        }

        private void _InitPlayerInputComponent()
        {
            if (AvatarInitParam.PlayerInputComponentInitParam.PlayerInputCandidate)
            {
                PlayerInputComponent = new PlayerInputComponent();
                PlayerInputComponent.Init(this, AvatarInitParam);
            }
        }

        private void _InitAnimatorComponent()
        {
            var mode = AvatarInitParam.CharacterControllerComponentInitParam.ControllerMode;
            if (mode == ControllerMode.None)
            {
            }
            else if (mode == ControllerMode.NativeAnimator)
            {
                CharacterControllerComponent = new AnimatorCharacterControllerComponent();
                CharacterControllerComponent.Init(this, AvatarInitParam);
            }
            else if (mode == ControllerMode.SimpleFSM)
            {
                CharacterControllerComponent = new SimpleFSMCharacterControllerComponent();
                CharacterControllerComponent.Init(this, AvatarInitParam);
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
            _InitPlayerInputComponent();
            _InitAnimatorComponent();
           
        }

        public override void Destroy()
        {
            CharacterControllerComponent?.Release();
            PlayerInputComponent?.Release();
            ModelComponent.Release();
            base.Destroy();
        }

        public override void Update(float dt)
        {
            PlayerInputComponent?.Update(dt);
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
        public PlayerInputComponent PlayerInputComponent;
        public CharacterControllerComponent CharacterControllerComponent;


    }
}