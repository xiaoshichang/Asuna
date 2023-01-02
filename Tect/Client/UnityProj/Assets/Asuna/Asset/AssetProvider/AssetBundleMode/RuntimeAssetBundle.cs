using UnityEngine;

namespace Asuna.Asset
{
    public class RuntimeAssetBundle
    {
        public RuntimeAssetBundle(string assetBundleName, AssetBundle assetBundle)
        {
            _AssetBundleName = assetBundleName;
            _AssetBundle = assetBundle;
        }

        public void Unload()
        {
            _AssetBundle.Unload(true);
            _AssetBundle = null;
        }

        public T LoadAsset<T>(string assetName) where T : Object
        {
            return _AssetBundle.LoadAsset<T>(assetName);
        }

        public AssetRequestAssetBundle<T> LoadAssetAsync<T>(string assetName) where T : Object
        {
            var req = _AssetBundle.LoadAssetAsync(assetName);
            return new AssetRequestAssetBundle<T>(assetName, req);
        }

        public void IncRefCounter()
        {
            _RefCounter += 1;
        }

        public void DecRefCounter()
        {
            _RefCounter -= 1;
        }

        public bool IsNoRef()
        {
            return _RefCounter == 0;
        }

        public string GetAssetBundleName()
        {
            return _AssetBundleName;
        }


        private readonly string _AssetBundleName;
        private int _RefCounter = 0;
        private AssetBundle _AssetBundle;
    }
}