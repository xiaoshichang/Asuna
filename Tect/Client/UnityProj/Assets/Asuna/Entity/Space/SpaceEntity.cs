namespace Asuna.Entity
{
    public partial class SpaceEntity : Entity
    {
        protected override string GetGameObjectName()
        {
            return $"Space-{Guid:B}";
        }

        public override void Init(object param)
        {
            base.Init(param);
        }

        public override void Destroy()
        {
            _DestroyAllLoadSceneItems();
            _ReleaseAllAssets();
            base.Destroy();
        }
    }
}