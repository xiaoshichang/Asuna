

using System;
using System.Collections.Generic;
using System.IO;
using Asuna.Application;
using Asuna.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asuna.Asset
{
    public partial class AssetProviderAssetBundle : IAssetProvider
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

        public override void DebugInfo()
        {
            ADebug.Info($"_AssetMap size : {_AssetMap.Count}");
            ADebug.Info($"_RuntimeAssetBundles size : {_RuntimeAssetBundles.Count}");
            foreach (var pair in _RuntimeAssetBundles)
            {
                var name = pair.Key;
                var rab = pair.Value;
                ADebug.Info($"\t name: {name}, ref counter: {rab.RefCounter()}");
            }
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

    }
}

