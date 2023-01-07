using UnityEngine;

namespace Asuna.Asset
{
    public class RuntimeAssetBundle
    {
        public RuntimeAssetBundle(AssetProviderAssetBundle provider, string assetBundleName, AssetBundle assetBundle)
        {
            _Provider = provider;
            _AssetBundleName = assetBundleName;
            _AssetBundle = assetBundle;
        }

        public void Unload()
        {
            _AssetBundle.Unload(true);
            _AssetBundle = null;
        }

        public T LoadAssetSync<T>(string assetID) where T : Object
        {
            return _AssetBundle.LoadAsset<T>(assetID);
        }

        public AssetBundleRequest LoadAssetAsync<T>(string assetID)
        {
            return _AssetBundle.LoadAssetAsync<T>(assetID);
        }

        public void IncRefCounter()
        {
            _RefCounter += 1;
        }

        public void DecRefCounter()
        {
            _RefCounter -= 1;
            if (_RefCounter == 0)
            {
                _Provider.ReleaseAssetBundle(this);
            }
        }

        public int RefCounter()
        {
            return _RefCounter;
        }

        public string GetAssetBundleName()
        {
            return _AssetBundleName;
        }

        private readonly AssetProviderAssetBundle _Provider;
        private readonly string _AssetBundleName;
        private int _RefCounter = 0;
        private AssetBundle _AssetBundle;
    }
}