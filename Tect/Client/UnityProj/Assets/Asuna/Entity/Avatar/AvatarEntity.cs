using System.Collections;

namespace Asuna.Entity
{

    public class AvatarEntity : Entity
    {
        protected override string GetGameObjectName()
        {
            return $"Avatar-{Guid:B}";
        }

        /// <summary>
        /// 初始化 - 数据部分
        /// </summary>
        /// <param name="param"> 初始化数据，类型为 AvatarData </param>
        public override void Init(object param)
        {
            base.Init(null);
            _AvatarData = param as AvatarData;
            _ModelCmpt.Init(this, param);
        }

        public override void Destroy()
        {
            _ModelCmpt.Release();
            base.Destroy();
        }
        
        /// <summary>
        /// 异步加载 - 资源部分
        /// </summary>
        public IEnumerator LoadAsync()
        {
            yield return _ModelCmpt.LoadModelAsync();
        }

        private AvatarData _AvatarData;
        private readonly ModelComponent _ModelCmpt = new();

    }
}