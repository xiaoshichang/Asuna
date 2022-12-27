using Asuna.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Asuna.UI
{
    
    public partial class UIManager
    {
        private void _InitHierarchy()
        {
            _InitRoot();
            _InitPageRoot();
            _InitBlockRoot();
            _InitPopupRoot();
        }

        private void _ReleaseHierarchy()
        {
            Object.DestroyImmediate(_Root);
        }

        private void _InitRoot()
        {
            _Root = new GameObject("UIRoot");
            _Root.layer = LayerMask.NameToLayer("UI");
            Object.DontDestroyOnLoad(_Root);

            _RootCanvas = _Root.AddComponent<Canvas>();
            _RootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                
            _RootCanvasScaler = _Root.AddComponent<CanvasScaler>();
            _RootGraphicRaycaster = _Root.AddComponent<GraphicRaycaster>();
        }

        private void _InitPageRoot()
        {
            _PageRoot = new GameObject("PageRoot");
            _PageRoot.layer = LayerMask.NameToLayer("UI");
            var rect = _PageRoot.AddComponent<RectTransform>();
            _PageRoot.transform.SetParent(_Root.transform);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(1, 1);
            rect.offsetMin = new Vector2(0, 0);
            rect.offsetMax = new Vector2(0, 0);
        }

        private void _InitBlockRoot()
        {
            _BlockRoot = new GameObject("BlockRoot");
            _BlockRoot.layer = LayerMask.NameToLayer("UI");
            var rect = _BlockRoot.AddComponent<RectTransform>();
            _BlockRoot.transform.SetParent(_Root.transform);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(1, 1);
            rect.offsetMin = new Vector2(0, 0);
            rect.offsetMax = new Vector2(0, 0);
        }

        private void _InitPopupRoot()
        {
            _PopupRoot = new GameObject("PopupRoot");
            _PopupRoot.layer = LayerMask.NameToLayer("UI");
            var rect = _PopupRoot.AddComponent<RectTransform>();
            _PopupRoot.transform.SetParent(_Root.transform);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(1, 1);
            rect.offsetMin = new Vector2(0, 0);
            rect.offsetMax = new Vector2(0, 0);
        }

        private GameObject _Root;
        private Canvas _RootCanvas;
        private CanvasScaler _RootCanvasScaler;
        private GraphicRaycaster _RootGraphicRaycaster;
        
        private GameObject _PageRoot;
        private GameObject _BlockRoot;
        private GameObject _PopupRoot;
        
    }
}