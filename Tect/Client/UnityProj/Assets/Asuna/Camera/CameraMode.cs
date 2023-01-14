namespace Asuna.Camera
{
    public abstract class CameraMode
    {
        public abstract void Enter();
        public abstract void Tick(float dt);
        public abstract void Leave();
    }
}