namespace Demo
{
    public abstract class DemoCaseBase
    {
        public abstract void InitDemo();
        public abstract void ReleaseDemo();
        public abstract void Tick(float dt); 
        public abstract string GetBtnName();
    }
}