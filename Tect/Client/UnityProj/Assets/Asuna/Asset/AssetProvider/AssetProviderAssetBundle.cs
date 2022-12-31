

using System;
using System.Collections.Generic;
using System.IO;
using Asuna.Utils;
using UnityEngine;

namespace Asuna.Asset
{
    public class AssetProviderAssetBundle : IAssetProvider
    {
        private static readonly string AssetBundleRoot = Path.Join(UnityEngine.Application.streamingAssetsPath, "AssetBundles");
        public static readonly string AssetBundleRootWindows = Path.Join(AssetBundleRoot, "windows");
        public static readonly string AssetBundleRootIOS = Path.Join(AssetBundleRoot, "ios");
        public static readonly string AssetBundleRootAndroid = Path.Join(AssetBundleRoot, "android");


        private string _GetRootManifestName()
        {
            if (UnityEngine.Application.platform == RuntimePlatform.WindowsPlayer
                || UnityEngine.Application.platform == RuntimePlatform.WindowsEditor)
            {
                return "windows";
            }
            else if (UnityEngine.Application.platform == RuntimePlatform.IPhonePlayer)
            {
                throw new NotImplementedException();
            }
            else if (UnityEngine.Application.platform == RuntimePlatform.Android)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private string _GetAssetBundleRootDir()
        {
            if (UnityEngine.Application.platform == RuntimePlatform.WindowsPlayer 
                || UnityEngine.Application.platform == RuntimePlatform.WindowsEditor)
            {
                return AssetBundleRootWindows;
            }
            else if (UnityEngine.Application.platform == RuntimePlatform.IPhonePlayer)
            {
                throw new NotImplementedException();
            }
            else if (UnityEngine.Application.platform == RuntimePlatform.Android)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private string _GetAssetBundlePathByName(string name)
        {
            var root = _GetAssetBundleRootDir();
            return Path.Join(root, name);
        }
        
        private void _InitAssetBundleManifest()
        {
            var rootManifestAssetBundlePath = _GetAssetBundlePathByName(_GetRootManifestName());
            _RootManifestAssetBundle = AssetBundle.LoadFromFile(rootManifestAssetBundlePath);
            _RootManifest = _RootManifestAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        private void _ReleaseAssetBundleManifest()
        {
            _RootManifest = null;
            _RootManifestAssetBundle.Unload(true);
            _RootManifestAssetBundle = null;
        }

        private void _InitAssetMap()
        {
            var allAssetBundles = _RootManifest.GetAllAssetBundles();
            // todo: multi thread
            foreach (var assetBundleName in allAssetBundles)
            {
                var assetBundlePath = _GetAssetBundlePathByName(assetBundleName);
                var assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
                foreach (var assetName in assetBundle.GetAllAssetNames())
                {
                    _AssetMap[assetName] = assetBundleName;
                }
                assetBundle.Unload(true);
            }
        }

        private void _ReleaseAssetMap()
        {
            _AssetMap.Clear();
        }
        
        public override void Init()
        {
            _InitAssetBundleManifest();
            _InitAssetMap();
        }

        
        public override void Release()
        {
            _ReleaseAssetMap();
            _ReleaseAssetBundleManifest();
        }

        private bool IsAssetBundleLoaded(string assetBundleName)
        {
            return _LoadedAssetBundles.ContainsKey(assetBundleName);
        }

        private void LoadAssetBundle(string assetBundleName)
        {
            if (_LoadedAssetBundles.ContainsKey(assetBundleName))
            {
                XDebug.Warning($"AssetBundle[{assetBundleName}] is loaded!");
                return;
            }

            var deps = _RootManifest.GetAllDependencies(assetBundleName);
            if (deps.Length > 0)
            {
                foreach (var dep in deps)
                {
                    LoadAssetBundle(dep);
                }
            }

            var path = _GetAssetBundlePathByName(assetBundleName);
            var Loaded = AssetBundle.LoadFromFile(path);
            _LoadedAssetBundles[assetBundleName] = Loaded;
        }

        private void ReleaseAssetBundle(string assetBundleName)
        {
            if (!_LoadedAssetBundles.TryGetValue(assetBundleName, out var assetBundle))
            {
                XDebug.Warning($"AssetBundle[{assetBundleName}] is not loaded!");
                return;
            }
            
            assetBundle.Unload(true);
            _LoadedAssetBundles.Remove(assetBundleName);
        }

        public override T LoadAssetSync<T>(string assetPath)
        {
            var assetName = assetPath.ToLower();
            
            if (!_AssetMap.TryGetValue(assetName, out var assetBundleName))
            {
                XDebug.Error($"asset not exist! {assetPath}");
                return null;
            }

            if (!IsAssetBundleLoaded(assetBundleName))
            {
                LoadAssetBundle(assetBundleName);
            }

            var bundle = _LoadedAssetBundles[assetBundleName];
            T asset = bundle.LoadAsset<T>(assetName);
            return asset;
        }

        /// <summary>
        /// RootManifest AssetBundle
        /// </summary>
        private AssetBundle _RootManifestAssetBundle;

        /// <summary>
        /// RootManifest
        /// </summary>
        private AssetBundleManifest _RootManifest;

        /// <summary>
        /// 资源字典 - 记录每一个资源对应的AssetBundle
        /// </summary>
        private readonly Dictionary<string, string> _AssetMap = new Dictionary<string, string>();

        /// <summary>
        /// 记录所有加载的 AssetBundle
        /// </summary>
        private readonly Dictionary<string, AssetBundle> _LoadedAssetBundles = new Dictionary<string, AssetBundle>();

    }
}

