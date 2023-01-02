using System;
using System.Collections.Generic;
using Asuna.Utils;
using Object = UnityEngine.Object;

namespace Asuna.Asset
{
    public partial class AssetProviderAssetBundle
    {
        /// <summary>
        /// AssetPath - 上层业务对资源ID的描述
        /// AssetName - AssetBundle对资源ID的描述
        /// </summary>
        private string _AssetPathToAssetID(string assetPath)
        {
            return assetPath.ToLower();
        }
        
        public override T LoadAssetSync<T>(string assetPath)
        {
            var assetID = _AssetPathToAssetID(assetPath);
            // check if asset exist
            if (!_AssetMap.TryGetValue(assetID, out var assetBundleName))
            {
                ADebug.Error($"asset not exist! {assetPath}");
                return null;
            }
            // check if AssetBundle loaded
            if (!_IsAssetBundleLoaded(assetBundleName))
            {
                _LoadAssetBundle(assetBundleName);
            }

            var bundle = _RuntimeAssetBundles[assetBundleName];
            T asset = bundle.LoadAsset<T>(assetID);
            _IncRuntimeAssetBundleRef(bundle);
            _LoadAssetMap[asset] = bundle;
            return asset;
        }

        public override AssetRequest<T> LoadAssetAsync<T>(string assetPath)
        {
            throw new NotImplementedException();
        }

        public override void ReleaseAsset(Object obj)
        {
            if (!_LoadAssetMap.TryGetValue(obj, out var rab))
            {
                ADebug.Warning($"{obj} is not loaded!");
                return;
            }

            _LoadAssetMap.Remove(obj);
            _DecRuntimeAssetBundleRef(rab);
        }

        private void _IncRuntimeAssetBundleRef(RuntimeAssetBundle rab)
        {
            rab.IncRefCounter();
        }

        private void _DecRuntimeAssetBundleRef(RuntimeAssetBundle rab)
        {
            rab.DecRefCounter();
            if (rab.IsNoRef())
            {
                _ReleaseAssetBundle(rab);
            }
        }
        
        /// <summary>
        /// 记录所有加载的资源和对应的AssetBundle
        /// </summary>
        private readonly Dictionary<Object, RuntimeAssetBundle> _LoadAssetMap = new Dictionary<Object, RuntimeAssetBundle>();
    }
}