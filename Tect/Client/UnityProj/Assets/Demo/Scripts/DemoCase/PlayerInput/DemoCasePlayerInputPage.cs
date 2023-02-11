﻿using Asuna.Application;
using Asuna.UI;
using TMPro;
using UnityEngine.UI;

namespace Demo.PlayerInput
{
    public class DemoCasePlayerInputPage : UIPage
    {
        public const string AssetPath = "Assets/Demo/Res/UI/LoadScenePage.prefab";
        
        public override void SetupController()
        {
            _FramerateText = _Seek<TMP_Text>("FrameRateText");
            _CloseBtn = _Seek<Button>("CloseBtn");
            _CloseBtn.onClick.AddListener(_OnClose);
        }
        
        public override void OnShow(ShowPageParam param)
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