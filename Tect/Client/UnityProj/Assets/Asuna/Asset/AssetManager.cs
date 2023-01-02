#if UNITY_EDITOR
using UnityEditor;
#endif

using Asuna.Interface;
using Object = UnityEngine.Object;


namespace Asuna.Asset
{
    public class AssetManager : IManager
    {
        
        private void _InitProvider()
        {
#if UNITY_EDITOR
            var mode = EditorPrefs.GetString(AssetProviderEditor.ProviderMode_Key);
            if (string.IsNullOrEmpty(mode) || mode == AssetProviderEditor.ProviderMode_Value_Editor)
            {
                _Provider = new AssetProviderEditor();
            }
            else
            {
                _Provider = new AssetProviderAssetBundle();
            }
#else
            _Provider = new AssetProviderAssetBundle();
#endif
            _Provider.Init();
        }
        
        
        public void Init(object param)
        {
            _InitProvider();
        }

        private void _ReleaseProvider()
        {
            _Provider.Release();
        }
        
        public void Release()
        {
            _ReleaseProvider();
        }
        
        public T LoadAssetSync<T>(string assetPath) where T : Object
        {
            return _Provider.LoadAssetSync<T>(assetPath);
        }

        public void ReleaseAsset(Object asset)
        {
            _Provider.ReleaseAsset(asset);
        }

        private IAssetProvider _Provider;

    }
}


