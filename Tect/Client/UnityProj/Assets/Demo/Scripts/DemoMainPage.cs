using System.Collections;
using System.Collections.Generic;
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
        }
    
        private Button _LoadSceneBtn;
        public const string AssetPath = "Assets/Demo/Res/UI/DemoMainPage.prefab";
    }

}
