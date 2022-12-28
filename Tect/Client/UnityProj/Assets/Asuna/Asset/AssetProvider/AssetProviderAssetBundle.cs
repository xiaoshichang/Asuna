

using System.Collections.Generic;
using Asuna.Utils;
using UnityEngine;

namespace Asuna.Asset
{
    public class AssetProviderAssetBundle : IAssetProvider
    {
        private void _InitAssetDict()
        {
        }
        
        public override void Init()
        {
            _InitAssetDict();
        }

        private void _ReleaseAssetDict()
        {
            
        }
        
        public override void Release()
        {
            _ReleaseAssetDict();
        }

        public override T LoadAssetSync<T>(string assetPath)
        {
            if (_AssetDict.TryGetValue(assetPath, out var assetBundle))
            {
                return assetBundle.LoadAsset<T>(assetPath);
            }

            XDebug.Error($"asset not exist! {assetPath}");
            return null;
        }


        /// <summary>
        /// 资源字典
        /// </summary>
        private readonly Dictionary<string, AssetBundle> _AssetDict = new Dictionary<string, AssetBundle>();

    }
}

