namespace Asuna.Camera
{
    public abstract class CameraMode
    {
        protected CameraMode(UnityEngine.Camera camera)
        {
            _Camera = camera;
        }
        
        public abstract void Enter();
        public abstract void Tick(float dt);
        public abstract void Leave();

        protected readonly UnityEngine.Camera _Camera;
    }
}