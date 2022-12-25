using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace AF.UI
{
    public partial class UIManager
    {
        private void _InitScreenFadeBlock()
        {
            _ScreenFadeBlockAsset = AssetManager.LoadAssetSync<GameObject>(ScreenFadeBlockAssetPath);
            var o = Object.Instantiate(_ScreenFadeBlockAsset.Result, _BlockRoot.transform);
            _ScreenFadeBlock = o.GetComponent<ScreenFadeBlock>();
        }


        private void _ReleaseScreenFadeBlock()
        {
            AssetManager.ReleaseAsset(_ScreenFadeBlockAsset);
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
        private const string ScreenFadeBlockAssetPath = "Assets/AsunaFramework/Res/UI/ScreenFadeBlock.prefab";
    }
}