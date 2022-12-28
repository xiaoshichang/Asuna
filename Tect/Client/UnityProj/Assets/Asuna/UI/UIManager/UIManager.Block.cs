using Asuna.Application;
using Asuna.Asset;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Asuna.UI
{
    public partial class UIManager
    {
        private void _InitScreenFadeBlock()
        {
            _ScreenFadeBlockAsset = G.AssetManager.LoadAssetSync<GameObject>(ScreenFadeBlockAssetPath);
            var o = Object.Instantiate(_ScreenFadeBlockAsset.Result, _BlockRoot.transform);
            _ScreenFadeBlock = o.GetComponent<ScreenFadeBlock>();
        }


        private void _ReleaseScreenFadeBlock()
        {
            G.AssetManager.ReleaseAsset(_ScreenFadeBlockAsset);
        }

        public void ScreenFadeTo(Color color, float interval=1)
        {
            _ScreenFadeBlock.FadeTo(color, interval);
        }

        public void ClearFade()
        {
            _ScreenFadeBlock.Clear();
        }

        private AsyncOperationHandle<GameObject> _ScreenFadeBlockAsset;
        private ScreenFadeBlock _ScreenFadeBlock;
        private const string ScreenFadeBlockAssetPath = "Assets/Asuna/Res/UI/ScreenFadeBlock.prefab";
    }
}