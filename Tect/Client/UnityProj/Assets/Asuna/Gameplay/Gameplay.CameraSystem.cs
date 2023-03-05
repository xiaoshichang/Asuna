using Asuna.Camera;
using Asuna.Foundation.Debug;

namespace Asuna.Gameplay
{
    public partial class GameplayInstance
    {
        private void _InitCameraSystem()
        {
            CameraSystem = new CameraSystem();
            CameraSystem.Init(null);
            ADebug.Info("Init Camera System Ok!");
        }

        private void _ReleaseCameraSystem()
        {
            CameraSystem.Release();
            CameraSystem = null;
            ADebug.Info("Release Camera System Ok!");
        }
        
        public CameraSystem CameraSystem;
    }
}