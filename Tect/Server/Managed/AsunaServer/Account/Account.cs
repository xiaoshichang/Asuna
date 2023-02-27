using AsunaServer.Application;
using AsunaServer.Debug;
using AsunaServer.Network;
using AsunaServer.Timer;
using AsunaShared.Message;

namespace AsunaServer.Account
{
    public enum AuthState
    {
        Ready,
        FindAccount,
        CheckOnline,
        Finish,
    }

    public delegate void AuthCallback(Account account);
    
    public class Account
    {
        public Account(LoginReq req, TcpSession session, AuthCallback callback)
        {
            _Request = req;
            _Session = session;
            _Callback = callback;
            _AuthState = AuthState.Ready;
        }

        public void Auth()
        {
            if (_AuthState != AuthState.Ready)
            {
                Logger.Error("unknown state");
                return;
            }

            _AuthState = AuthState.FindAccount;
            TimerMgr.AddTimer(1000, OnFindAccount);
        }

        private void OnFindAccount(object? param)
        {
            if (_Request.Username != "xiao")
            {
                _AuthState = AuthState.Finish;
                _LoginResult = LoginRetCode.NotExit;
                _Callback?.Invoke(this);
                _Callback = null;
                return;
            }
            _AuthState = AuthState.CheckOnline;
            G.CallStub("LoginStub", "_OnCheckAccountLogin", new object[]{1, "hello"});
        }

        public void Send(object message)
        {
            _Session.Send(message);
        }

        private readonly LoginReq _Request;
        public string Username => _Request.Username;
        private readonly TcpSession _Session;
        private AuthCallback? _Callback;

        private AuthState _AuthState;
        public AuthState AuthState => _AuthState;
        private LoginRetCode _LoginResult;
        public LoginRetCode LoginResult => _LoginResult;
        
        
    }
}

