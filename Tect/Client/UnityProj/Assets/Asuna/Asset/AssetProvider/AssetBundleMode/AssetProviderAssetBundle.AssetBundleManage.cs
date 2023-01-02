using System.Collections.Generic;
using Asuna.Utils;
using UnityEngine;

namespace Asuna.Asset
{
    public partial class AssetProviderAssetBundle
    {
        private bool _IsAssetBundleLoaded(string assetBundleName)
        {
            return _RuntimeAssetBundles.ContainsKey(assetBundleName);
        }

        private void _LoadAllDepAssetBundle(string assetBundleName)
        {
            var deps = _RootManifest.GetAllDependencies(assetBundleName);
            if (deps.Length > 0)
            {
                foreach (var dep in deps)
                {
                    var depBundle = _LoadAssetBundle(dep);
                    _IncRuntimeAssetBundleRef(depBundle);
                }
            }
        }
        
        private RuntimeAssetBundle _LoadAssetBundle(string assetBundleName)
        {
            if (_RuntimeAssetBundles.TryGetValue(assetBundleName, out var rab))
            {
                ADebug.Warning($"{assetBundleName} is loaded!");
                return rab;
            }

            _LoadAllDepAssetBundle(assetBundleName);

            var path = _GetAssetBundlePathByName(assetBundleName);
            var ab = AssetBundle.LoadFromFile(path);
            var bundle = new RuntimeAssetBundle(assetBundleName, ab);
            _RuntimeAssetBundles[assetBundleName] = bundle;
            return bundle;
        }

        private void _ReleaseAllDepAssetBundle(string assetBundleName)
        {
            var deps = _RootManifest.GetAllDependencies(assetBundleName);
            if (deps.Length > 0)
            {
                foreach (var dep in deps)
                {
                    var depBundle = _RuntimeAssetBundles[dep];
                    _DecRuntimeAssetBundleRef(depBundle);
                }
            }
        }

        private void _ReleaseAssetBundle(RuntimeAssetBundle rab)
        {
            var name = rab.GetAssetBundleName();
            rab.Unload();
            _RuntimeAssetBundles.Remove(name);
            _ReleaseAllDepAssetBundle(name);
        }
        
        /// <summary>
        /// 记录所有加载的 AssetBundle
        /// </summary>
        private readonly Dictionary<string, RuntimeAssetBundle> _RuntimeAssetBundles = new Dictionary<string, RuntimeAssetBundle>();
    }
}