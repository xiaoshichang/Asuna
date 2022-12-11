using UnityEngine;
using UnityEngine.UI;

namespace AsunaClient.Foundation.UI
{
    
    public partial class UIManager
    {
        private void _InitHierarchy()
        {
            _InitRoot();
            _InitPopupRoot();
            _InitPageRoot();
        }

        private void _ReleaseHierarchy()
        {
            Object.DestroyImmediate(_Root);
        }

        private void _InitRoot()
        {
            _Root = new GameObject("UIRoot");
            Object.DontDestroyOnLoad(_Root);

            _RootCanvas = _Root.AddComponent<Canvas>();
            _RootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                
            _RootCanvasScaler = _Root.AddComponent<CanvasScaler>();
            _RootGraphicRaycaster = _Root.AddComponent<GraphicRaycaster>();
        }

        private void _InitPageRoot()
        {
            _PageRoot = new GameObject("PageRoot");
            _PageRoot.transform.parent = _Root.transform;
        }

        private void _InitPopupRoot()
        {
            _PopupRoot = new GameObject("PopupRoot");
            _PopupRoot.transform.parent = _Root.transform;

        }

        private GameObject _Root;
        private Canvas _RootCanvas;
        private CanvasScaler _RootCanvasScaler;
        private GraphicRaycaster _RootGraphicRaycaster;
        
        private GameObject _PageRoot;
        private GameObject _PopupRoot;
        
    }
}