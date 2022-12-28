#if UNITY_EDITOR

using UnityEditor;

namespace Asuna.Asset
{
    public class AssetProviderEditor : IAssetProvider
    {
        public const string ProviderMode_Key = "_Key_ProviderMode";
        public const string ProviderMode_Value_Editor = "editor";
        public const string ProviderMode_Value_AB = "ab";
        
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
    }
}

#endif