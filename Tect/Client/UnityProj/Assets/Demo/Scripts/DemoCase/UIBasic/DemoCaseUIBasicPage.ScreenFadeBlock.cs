using Asuna.Application;
using Asuna.Timer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Demo.UIBasic
{
    public partial class DemoCaseUIBasicPage
    {
        private void _SetupScreenFadeBlock()
        {
            var go = Instantiate(_BtnTemplate, _Root.transform);
            go.SetActive(true);
            var btn = go.GetComponent<Button>();
            btn.onClick.AddListener(_OnScreenFadeStart);
            var btnText = btn.gameObject.GetComponentInChildren<TMP_Text>();
            btnText.text = "Screen Fade";
        }

        private void _OnScreenFadeStart()
        {
            TimerMgr.RegisterTimer(2000, _ClearScreenFade);
            G.UIManager.ScreenFadeTo(Color.black, 2);
        }

        private void _ClearScreenFade(object arg)
        {
            G.UIManager.ClearFade(2);
        }
        
    }
}