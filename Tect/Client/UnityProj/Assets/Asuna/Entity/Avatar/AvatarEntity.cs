using System.Collections;

namespace Asuna.Entity
{

    public class AvatarEntity : Entity
    {
        protected override string GetGameObjectName()
        {
            return $"Avatar-{Guid:B}";
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Destroy()
        {
            base.Destroy();
        }
        
        public IEnumerator LoadAsync()
        {
            yield return null;
        }
    }
}