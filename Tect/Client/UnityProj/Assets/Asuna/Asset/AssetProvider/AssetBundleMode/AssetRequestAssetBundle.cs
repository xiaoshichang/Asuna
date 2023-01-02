using System.Collections;
using UnityEngine;


namespace Asuna.Asset
{
    public class AssetRequestAssetBundle<T> : AssetRequest<T> where T : Object
    {
        public AssetRequestAssetBundle(string assetPath, AssetBundleRequest request) : base(assetPath)
        {
            _Request = request;
        }

        private readonly AssetBundleRequest _Request;
        public override YieldInstruction Operation => _Request;
        public override T Asset => _Request.asset as T;
    }
}