using System.Collections;
using System.Collections.Generic;
using Asuna.Application;
using Asuna.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Demo.Moba
{
    public class NetworkConnectPage : UIPage
    {
        public const string AssetPath = "Assets/Demo/Res/UI/MobaLoginPage.prefab";
    
        public override void SetupController()
        {
            _UsernameInput = _Seek<TMP_InputField>("UsernameInputField");
            _UsernameInput.text = "xiao";
            _PasswordInput = _Seek<TMP_InputField>("PasswordInputField");
            _PasswordInput.text = "pass";
            _ServerInput = _Seek<TMP_InputField>("ServerInputField");
            _ServerInput.text = "127.0.0.1:50001";
            _LoginButton = _Seek<Button>("LoginButton");
            _LoginButton.onClick.AddListener(_OnLoginButtonClick);
            _CloseButton = _Seek<Button>("CloseButton");
            _CloseButton.onClick.AddListener(_OnCloseButtonClick);
            _LoginResult = _Seek<TMP_Text>("LoginRetLabel");
        }

        public override void OnShow(ShowPageParam param)
        {
        }

        private void _OnLoginButtonClick()
        {
            var gameplay = G.GameplayInstance as DemoGameplayInstance;
            var demo = gameplay.GetCurrentRunningDemo() as DemoCaseNetworkConnect;
            _LoginResult.text = string.Empty;
            demo.LoginToServer(_UsernameInput.text, _PasswordInput.text, _ServerInput.text, _OnLoginToServerResult);
        }

        private void _OnLoginToServerResult(string result)
        {
            _LoginResult.text = result;
        }

        private void _OnCloseButtonClick()
        {
            var gameplay = G.GameplayInstance as DemoGameplayInstance;
            gameplay.ExitCurrentRunningDemo();
        }

        private TMP_InputField _UsernameInput;
        private TMP_InputField _PasswordInput;
        private TMP_InputField _ServerInput;
        private Button _LoginButton;
        private Button _CloseButton;
        private TMP_Text _LoginResult;

    }
}

