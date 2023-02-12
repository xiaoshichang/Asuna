using AsunaServer.Debug;
using AsunaServer.Network;
using AsunaServer.Timer;

namespace AsunaServer.Account
{
    public enum AuthState
    {
        NotAuth,
        AuthSuccess,
        AuthFail
    }
    
    
    public delegate void OnAuthResultCallback(Account account);
    
    
    public class Account
    {
        public Account(TcpSession session)
        {
            _AuthState = AuthState.NotAuth;
            _Session = session;
        }

        public void Auth(string username, string password, OnAuthResultCallback callback)
        {
            if (_AuthState != AuthState.NotAuth)
            {
                Logger.Error("unknown state");
                return;
            }
            Username = username;
            _Callback = callback;
            TimerMgr.AddTimer(1000, OnAuth);
        }

        private void OnAuth(object? param)
        {
            if (Username == "xiao")
            {
                _AuthState = AuthState.AuthSuccess;
                _Callback?.Invoke(this);
            }
            else
            {
                _AuthState = AuthState.AuthFail;
                _Callback?.Invoke(this);
            }

            _Callback = null;
        }

        public AuthState GetAuthState()
        {
            return _AuthState;
        }

        public void Send(object message)
        {
            _Session?.Send(message);
        }

        public string? Username;
        private AuthState _AuthState;
        private readonly TcpSession _Session;
        private OnAuthResultCallback? _Callback;
    }
}

