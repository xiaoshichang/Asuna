using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Asuna.Asset
{
    public abstract class IAssetProvider
    {
        public const string ProviderMode_Key = "_Key_ProviderMode";
        public const string ProviderMode_Value_Editor = "editor";
        public const string ProviderMode_Value_AB = "ab";
        
        public abstract void Init();
        public abstract void Release();
        public abstract T LoadAssetSync<T>(string assetPath) where T : Object;
        public abstract AssetRequest<T> LoadAssetAsync<T>(string assetPath) where T : Object;
        public abstract void ReleaseAsset(Object obj);
    }

}