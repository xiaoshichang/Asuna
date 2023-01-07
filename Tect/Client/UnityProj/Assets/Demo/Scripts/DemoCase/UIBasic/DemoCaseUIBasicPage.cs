using Asuna.Application;
using Asuna.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Demo.UIBasic
{
    public partial class DemoCaseUIBasicPage : UIPage
    {
        public const string AssetPath = "Assets/Demo/Res/UI/UIBasicPage.prefab";

        public override void SetupController()
        {
            _CloseBtn = _Root.transform.Find("CloseBtn").GetComponent<Button>();
            _BtnTemplate = _Root.transform.Find("BtnTemplate").gameObject;
            _BtnTemplate.SetActive(false);
        }

        private void _SetupCloseBtn()
        {
            _CloseBtn.onClick.AddListener(_OnClose);
        }

        private void _OnClose()
        {
            var gameplay = G.GameplayInstance as DemoGameplayInstance;
            gameplay.ExitCurrentRunningDemo();
        }
        
        public override void OnShow(ShowPageParam param)
        {
            _SetupCloseBtn();
            _SetupScreenFadeBlock();
        }

        private Button _CloseBtn;
        private GameObject _BtnTemplate;
    }
}