namespace Asuna.Entity
{
    public abstract class Component
    {
        public abstract void Init(Entity owner, object param);
        public abstract void Release();
    }
}