using System;
using System.Collections;
using System.Collections.Generic;
using Asuna.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asuna.Asset
{
    public partial class AssetProviderAssetBundle
    {
        /// <summary>
        /// AssetPath - 上层业务对资源ID的描述，一般为资源路径
        /// AssetID - AssetBundle对资源ID的描述，全小写
        /// </summary>
        public string AssetPathToAssetID(string assetPath)
        {
            return assetPath.ToLower();
        }

        /// <summary>
        /// 根据 AssetID 获取所属的 AssetBundle
        /// </summary>
        public string GetAssetBundleByAssetID(string assetID)
        {
            if (!_AssetMap.TryGetValue(assetID, out var assetBundleName))
            {
                return "";
            }
            return assetBundleName;
        }

        public override AssetRequestHandler<T> LoadAsset<T>(AssetRequest request)
        {
            var handler = new AssetRequestHandlerAssetBundle<T>(request, this);
            return handler;
        }

        public override void ReleaseAsset(AssetRequestHandler handler)
        {
            handler.OnReleaseAsset();
        }
    }
}