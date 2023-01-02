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

        public override T LoadAssetSync<T>(string assetPath)
        {
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
        }

        public override AssetRequest<T> LoadAssetAsync<T>(string assetPath)
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            var request = new AssetRequestEditorSim<T>(assetPath, asset);
            return request;
        }

        public override void ReleaseAsset(Object obj)
        {
        }
    }
}

#endif