using Asuna.Application;
using Asuna.UI;
using TMPro;
using UnityEngine.UI;

namespace Demo.LoadScene
{
    public class DemoCaseLoadScenePage : UIPage
    {
        public const string AssetPath = "Assets/Demo/Res/UI/LoadScenePage.prefab";
        
        public override void SetupController()
        {
            _FramerateText = _Root.transform.Find("FrameRateText").GetComponent<TMP_Text>();
            _CloseBtn = _Root.transform.Find("CloseBtn").GetComponent<Button>();
            _CloseBtn.onClick.AddListener(_OnClose);
        }
        
        public override void OnShow(ShowPageParam param)
        {
        }

        public override void OnHide()
        {
        }

        private void _OnClose()
        {
            var gameplay = G.GameplayInstance as DemoGameplayInstance;
            gameplay.ExitCurrentRunningDemo();
        }

        private TMP_Text _FramerateText;
        private Button _CloseBtn;
    }
}