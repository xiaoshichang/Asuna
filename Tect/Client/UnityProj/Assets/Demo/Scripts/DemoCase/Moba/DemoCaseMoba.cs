﻿using Asuna.Application;
using Asuna.Network;
using Asuna.Utils;
using AsunaShared.Message;

namespace Demo.Moba
{
    public delegate void OnConnectToServerResult(string result);
    
    
    public class DemoCaseMoba : DemoCaseBase
    {
        public override void InitDemo()
        {
            G.UIManager.ShowPage(nameof(MobaLoginPage), null);
        }

        public override void ReleaseDemo()
        {
            G.UIManager.HidePage(nameof(MobaLoginPage));
        }

        public override void Tick(float dt)
        {
        }

        public override string GetDemoName()
        {
            return "Moba";
        }

        public void LoginToServer(string username, string password, string server, OnConnectToServerResult callback)
        {
            ADebug.Assert(_OnConnectToServerResult == null);

            var endpoint = NetworkHelper.ParseIPEndPoint(server);
            
            _OnConnectToServerResult = callback;
            _Username = username;
            _Password = password;
            G.NetworkManager.ConnectToAsync(endpoint.Address.ToString(), endpoint.Port, _OnConnectCallback);
        }

        private void _OnConnectCallback(OnConnectResult cr)
        {
            ADebug.Assert(_OnConnectToServerResult != null);
            
            if (cr == OnConnectResult.OK)
            {
                _LoginToServer();
            }
            else
            {
                _OnConnectToServerResult.Invoke("connect to server fail");
                _OnConnectToServerResult = null;
            }
        }

        private void _LoginToServer()
        {
            ADebug.Assert(_OnConnectToServerResult != null);
            
            G.NetworkManager.RegisterMessageHandler(typeof(LoginRsp), _OnLoginToServer);
            var loginReq = new LoginReq()
            {
                Password = _Password,
                Username = _Username
            };
            G.NetworkManager.Send(loginReq);
        }

        private void _OnLoginToServer(object message)
        {
            G.NetworkManager.UnRegisterMessageHandler(typeof(LoginRsp), _OnLoginToServer);
            var rsp = message as LoginRsp;
            if (rsp.Username != _Username)
            {
                _OnConnectToServerResult.Invoke("unknown error");
                _Username = null;
                _Password = null;
                return;
            }
            
            
            if (rsp.RetCode == LoginRetCode.Ok)
            {
                _OnConnectToServerResult.Invoke("Login Result: OK");
            }
            else
            {
                _OnConnectToServerResult.Invoke("Login Result: Fail");
            }

            _OnConnectToServerResult = null;
            _Username = null;
            _Password = null;
        }

        private string _Username;
        private string _Password;
        private OnConnectToServerResult _OnConnectToServerResult;
    }
}