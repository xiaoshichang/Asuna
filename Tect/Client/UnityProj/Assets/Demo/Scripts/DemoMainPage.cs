using Asuna.Application;
using Asuna.UI;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

namespace Demo
{
    public class DemoMainPage : UIPage
    {

        public override void SetupController()
        {
            _BtnTemplate = _Root.transform.Find("BtnTemplate").gameObject;
            _BtnTemplate.SetActive(false);
        }

        public override void OnShow(ShowPageParam param)
        {
            var gameplay = G.GameplayInstance as DemoGameplayInstance;
            var allDemos = gameplay.GetAllDemos();
            foreach (var pair in allDemos)
            {
                var go = Instantiate(_BtnTemplate, _Root.transform);
                go.SetActive(true);
                var btn = go.GetComponent<Button>();
                btn.onClick.AddListener(delegate { _OnBtnClick(pair.Key); });
                var text = btn.GetComponentInChildren<TMP_Text>();
                text.text = pair.Key;
            }
        }

        private void _OnBtnClick(string demo)
        {
            var gameplay = G.GameplayInstance as DemoGameplayInstance;
            gameplay.EnterDemo(demo);
        }

        private GameObject _BtnTemplate;
        public const string AssetPath = "Assets/Demo/Res/UI/DemoMainPage.prefab";
    }

}
