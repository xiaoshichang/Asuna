using System;
using Asuna.Application;
using Asuna.Auth;
using Asuna.Foundation.Debug;
using Asuna.UI;
using AsunaShared.Message;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Demo
{
    
    public class LoginPage : UIPage
    {
        public const string AssetPath = "Assets/Demo/Res/UI/LoginPage.prefab";
    
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

            _AvatarSelectPanel = _Seek("AvatarSelectPanel");
            _avatarItemViewTmp = _Seek<AvatarSelectItemView>("AvatarSelectPanel/AvatarList/Viewport/Content/AvatarItem");
            
            _AvatarList = _Seek<Listview>("AvatarSelectPanel/AvatarList");
            _AvatarList.SetViewTemplate(_avatarItemViewTmp);

            _SelectedAvatar = _Seek<TMP_Text>("AvatarSelectPanel/SelectedAvatar");
            
            _SelectButton = _Seek<Button>("AvatarSelectPanel/SelectButton");
            _SelectButton.onClick.AddListener(_OnSelectAvatarBtn);
        }

        public override void OnShow(ShowPageParam param)
        {
            _AvatarSelectPanel.SetActive(false);
            _SelectedAvatar.text = string.Empty;
        }

        private void _OnLoginButtonClick()
        {
            var gameplay = G.GameplayInstance as DemoGameplayInstance;
            _LoginResult.text = string.Empty;
            gameplay.LoginToServer(_UsernameInput.text, _PasswordInput.text, _ServerInput.text, _OnLoginToServerResult);
        }

        private void _ShowAvatarList(Account account)
        {
            foreach (var avatarID in account.AvatarList)
            {
                var item = _AvatarList.AddItem(avatarID);
                item.OnSelectCallback = _OnSelectAvatarItemCallback;
            }
        }
        
        private void _OnLoginToServerResult(AuthRsp rsp)
        {
            if (rsp == null)
            {
                _LoginResult.text = "connect fail";
                return;
            }

            switch (rsp.Ret)
            {
                case AuthRetCode.NotExit:
                {
                    _LoginResult.text = "account not exist";
                    return;
                }
                case AuthRetCode.Ok:
                {
                    var account = new Account(rsp.Data);
                    G.Account = account;
                
                    _AvatarSelectPanel.SetActive(true);
                    _ShowAvatarList(account);
                    return;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void _OnCloseButtonClick()
        {
            Application.Quit(0);
        }

        private void _OnSelectAvatarItemCallback(ListviewItemView i)
        {
            var detail = (Guid)i.GetModel();
            _SelectedAvatar.text = detail.ToString();
        }

        private void _OnSelectAvatarBtn()
        {
            ADebug.Assert(G.Account != null);
            if (_AvatarList.CurrentSelectedItem == null)
            {
                return;
            }

            var avatar = (Guid)_AvatarList.CurrentSelectedItem.GetModel();
            G.Account.CallServer("SelectAvatar", new object[]{avatar});
        }

        private TMP_InputField _UsernameInput;
        private TMP_InputField _PasswordInput;
        private TMP_InputField _ServerInput;
        private Button _LoginButton;
        private Button _CloseButton;
        private Button _SelectButton;
        private TMP_Text _LoginResult;
        private TMP_Text _SelectedAvatar;
        private GameObject _AvatarSelectPanel;
        private Listview _AvatarList;
        private AvatarSelectItemView _avatarItemViewTmp;

    }
}

