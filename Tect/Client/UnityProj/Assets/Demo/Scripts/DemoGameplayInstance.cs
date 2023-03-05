
using Asuna.Application;
using Asuna.Auth;
using Asuna.Foundation.Debug;
using Asuna.Gameplay;
using Asuna.Network;
using AsunaShared.Message;

namespace Demo
{
    public delegate void OnAccountAuthResult(AuthRsp rsp);

    
    public class DemoGameplayInstance : GameplayInstance
    {
        public override void Update(float dt)
        {
        }

        public override void EntryGameplay()
        {
            G.UIManager.ShowPage(nameof(LoginPage), null);
        }

         public void LoginToServer(string username, string password, string server, OnAccountAuthResult callback)
        {
            ADebug.Assert(_OnAccountAuthResult == null);
            ADebug.Assert(_AuthReq == null);

            var endpoint = NetworkHelper.ParseIPEndPoint(server);
            
            _OnAccountAuthResult = callback;
            _AuthReq = new AuthReq()
            {
                Username = username,
                Password = password
            };
            G.NetworkManager.ConnectToAsync(endpoint.Address.ToString(), endpoint.Port, _OnConnectCallback);
        }

        private void _OnConnectCallback(OnConnectResult cr)
        {
            ADebug.Assert(_OnAccountAuthResult != null);
            
            if (cr == OnConnectResult.OK)
            {
                _LoginToServer();
            }
            else
            {
                _OnAccountAuthResult.Invoke(null);
                _OnAccountAuthResult = null;
            }
        }

        private void _LoginToServer()
        {
            ADebug.Assert(_OnAccountAuthResult != null);
            G.NetworkManager.RegisterMessageHandler(typeof(AuthRsp), _OnLoginToServer);
            G.NetworkManager.Send(_AuthReq);
        }

        
        private void _OnLoginToServer(object message)
        {
            G.NetworkManager.UnRegisterMessageHandler(typeof(AuthRsp), _OnLoginToServer);
            var rsp = message as AuthRsp;
            _OnAccountAuthResult.Invoke(rsp);
            _OnAccountAuthResult = null;
            _AuthReq = null;
        }

        private AuthReq _AuthReq;
        private OnAccountAuthResult _OnAccountAuthResult;

    }
}