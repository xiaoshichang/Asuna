using UnityEngine;
using UnityEngine.UI;

namespace Asuna.Foundation
{
    public enum UILayer
    {
        Page,
        Popup
    }
    
    public static partial class UIManager
    {
        private static void _InitHierarchy()
        {
            _InitRoot();
            _InitPopupRoot();
            _InitPageRoot();
        }

        private static void _ReleaseHierarchy()
        {
            Object.DestroyImmediate(_Root);
        }

        private static void _InitRoot()
        {
            _Root = new GameObject("UIRoot");
            Object.DontDestroyOnLoad(_Root);

            _RootCanvas = _Root.AddComponent<Canvas>();
            _RootCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                
            _RootCanvasScaler = _Root.AddComponent<CanvasScaler>();
            _RootGraphicRaycaster = _Root.AddComponent<GraphicRaycaster>();
        }

        private static void _InitPageRoot()
        {
            _PageRoot = new GameObject("PageRoot");
            _PageRoot.transform.parent = _Root.transform;
        }

        private static void _InitPopupRoot()
        {
            _PopupRoot = new GameObject("PopupRoot");
            _PopupRoot.transform.parent = _Root.transform;

        }

        private static GameObject _Root;
        private static Canvas _RootCanvas;
        private static CanvasScaler _RootCanvasScaler;
        private static GraphicRaycaster _RootGraphicRaycaster;
        
        private static GameObject _PageRoot;
        private static GameObject _PopupRoot;
        
    }
}