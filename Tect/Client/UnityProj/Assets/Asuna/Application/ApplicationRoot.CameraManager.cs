using Asuna.Camera;
using Asuna.Utils;

namespace Asuna.Application
{
    public partial class ApplicationRoot
    {
        private void _InitCameraManager()
        {
            CameraManager = new CameraManager();
            CameraManager.Init(null);
            ADebug.Info("Init CameraManager Ok!");
        }

        private void _ReleaseCameraManager()
        {
            CameraManager.Release();
            CameraManager = null;
            ADebug.Info("Release CameraManager Ok!");
        }
        
        public CameraManager CameraManager;
    }
    
}