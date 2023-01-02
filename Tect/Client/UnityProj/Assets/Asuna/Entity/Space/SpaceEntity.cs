namespace Asuna.Entity
{
    public partial class SpaceEntity : Entity
    {
        protected override string GetGameObjectName()
        {
            return $"Space-{Guid:B}";
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Destroy()
        {
            _DestroyAllLoadSceneItems();
            _ReleaseAllAssets();
            base.Destroy();
        }
    }
}