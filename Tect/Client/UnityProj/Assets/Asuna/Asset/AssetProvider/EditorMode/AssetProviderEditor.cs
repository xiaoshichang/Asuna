#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Asuna.Asset
{
    public class AssetProviderEditor : IAssetProvider
    {
        
        public override void Init()
        {
        }

        public override void Release()
        {
        }
        

        public override AssetRequestHandler<T> LoadAsset<T>(AssetRequest request)
        {
            var handler = new AssetRequestHandlerEditorSim<T>(request);
            return handler;
        }

        public override void ReleaseAsset(AssetRequestHandler handler)
        {
        }

        public override void DebugInfo()
        {
        }
    }
}

#endif