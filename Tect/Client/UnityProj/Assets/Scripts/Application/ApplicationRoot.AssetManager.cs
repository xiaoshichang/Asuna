using AsunaClient.Foundation;
using AsunaClient.Foundation.Asset;
using UnityEditor.VersionControl;

namespace AsunaClient.Application
{
    public partial class ApplicationRoot
    {
        private void _InitAssetManager()
        {
            AssetManager.Init();
            XDebug.Info("Init Asset Manager OK!");
        }

        private void _ReleaseAssetManager()
        {
            AssetManager.Release();
        }

        public readonly AssetManager AssetManager = new AssetManager();
    }
}