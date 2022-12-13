
using AsunaClient.Foundation;
using AsunaClient.Foundation.Scene;

namespace AsunaClient.Application
{
    public partial class ApplicationRoot
    {
        private void _InitSceneManager()
        {
            _SceneManager = new SceneManager();
            _SceneManager.Init();
            XDebug.Info("Init Scene Manager Ok!");
        }

        private void _ReleaseSceneManager()
        {
            _SceneManager.Release();
            _SceneManager = null;
            XDebug.Info("Release Scene Manager Ok!");
        }

        public SceneManager _SceneManager;
    }
}