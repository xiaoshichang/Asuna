using Asuna.Camera;
using Asuna.Utils;

namespace Asuna.Gameplay
{
    public partial class GameplayInstance
    {
        private void _InitCameraSystem()
        {
            CameraSystem = new CameraSystem();
            CameraSystem.Init(null);
            ADebug.Info("Init CameraManager Ok!");
        }

        private void _ReleaseCameraSystem()
        {
            CameraSystem.Release();
            CameraSystem = null;
            ADebug.Info("Release CameraManager Ok!");
        }
        
        public CameraSystem CameraSystem;
    }
}