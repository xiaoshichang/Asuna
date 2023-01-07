#if UNITY_EDITOR
using UnityEditor;
#endif

using Asuna.Application;
using Asuna.Application.GM;
using Asuna.Interface;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Asuna.Asset
{
    public class AssetManager : IManager
    {
        
        private void _InitProvider()
        {
#if UNITY_EDITOR
            var mode = EditorPrefs.GetString(IAssetProvider.ProviderMode_Key);
            if (string.IsNullOrEmpty(mode) || mode == IAssetProvider.ProviderMode_Value_Editor)
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

        public AssetRequestHandler<T> LoadAsset<T>(string assetPath) where T : Object
        {
            var request = new AssetRequest()
            {
                AssetPath = assetPath
            };
            return LoadAsset<T>(request);
        }
        
        public AssetRequestHandler<T> LoadAsset<T>(AssetRequest request) where T : Object
        {
            var handler = _Provider.LoadAsset<T>(request);
            return handler;
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        public void ReleaseAsset(AssetRequestHandler handler)
        {
            _Provider.ReleaseAsset(handler);
        }

        [GM("asset.debug", "debug asset manager internal info")]
        public static void DebugInfo()
        {
            G.AssetManager._Provider.DebugInfo();
        }
        

        private IAssetProvider _Provider;

    }
}


