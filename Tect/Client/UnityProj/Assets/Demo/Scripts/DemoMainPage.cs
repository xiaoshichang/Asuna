using System.Collections;
using System.Collections.Generic;
using AF.Application;
using AF.Timer;
using AF.UI;
using AF.Utils;
using UnityEngine;

using UnityEngine.UI;

namespace Demo
{
    public class DemoMainPage : UIPage
    {

        public override void SetupController()
        {
            _LoadSceneBtn = _Root.transform.Find("LoadSceneBtn").GetComponent<Button>();
            _LoadSceneBtn.onClick.AddListener(_OnLoadScene);
        }

        public override void OnShow(ShowPageParam param)
        {
        }

        public override void OnHide()
        {
        }
        
        private void _OnLoadScene()
        {
            XDebug.Info("load scene");
            G.UIManager.ScreenFadeTo(Color.black);
            TimerMgr.RegisterTimer(2000, _ResetFade);
        }

        private void _ResetFade(object param)
        {
            G.UIManager.ClearFade();
        }
    
        private Button _LoadSceneBtn;
        public const string AssetPath = "Assets/Demo/Res/UI/DemoMainPage.prefab";
    }

}
