using AsunaClient.Foundation;
using AsunaClient.Foundation.Asset;
using UnityEditor.VersionControl;

namespace AsunaClient.Application
{
    public partial class ApplicationRoot
    {
        private void _InitAssetManager()
        {
            AssetManager = new AssetManager();
            AssetManager.Init(null);
            XDebug.Info("Init Asset Manager Ok!");
        }

        private void _ReleaseAssetManager()
        {
            AssetManager.Release();
            AssetManager = null;
            XDebug.Info("Release Asset Manager Ok!");
        }

        public AssetManager AssetManager;
    }
}