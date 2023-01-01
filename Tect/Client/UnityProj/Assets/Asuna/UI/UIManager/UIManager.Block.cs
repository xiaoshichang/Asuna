using Asuna.Application;
using Asuna.Asset;
using UnityEngine;

namespace Asuna.UI
{
    public partial class UIManager
    {
        private void _InitScreenFadeBlock()
        {
            _ScreenFadeBlockAsset = G.AssetManager.LoadAssetSync<GameObject>(ScreenFadeBlockAssetPath);
            var o = Object.Instantiate(_ScreenFadeBlockAsset, _BlockRoot.transform);
            _ScreenFadeBlock = o.GetComponent<ScreenFadeBlock>();
        }


        private void _ReleaseScreenFadeBlock()
        {
            Object.DestroyImmediate(_ScreenFadeBlock);
            _ScreenFadeBlock = null;
            
            G.AssetManager.ReleaseAsset(_ScreenFadeBlockAsset);
            _ScreenFadeBlockAsset = null;
        }

        public void ScreenFadeTo(Color color, float intervalIsSecond=1)
        {
            _ScreenFadeBlock.FadeTo(color, intervalIsSecond);
        }

        public void ClearFade(float intervalIsSecond=1)
        {
            _ScreenFadeBlock.Clear(intervalIsSecond);
        }

        private GameObject _ScreenFadeBlockAsset;
        private ScreenFadeBlock _ScreenFadeBlock;
        private const string ScreenFadeBlockAssetPath = "Assets/Asuna/Res/UI/ScreenFadeBlock.prefab";
    }
}