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
        public Account(string username, string password, TcpSession session)
        {
            _Username = username;
            _Password = password;
            _Session = session;
            _Session.OnDisconnectHandler += _OnSessionDisconnect;
        }

        public void Auth(OnAuthResultCallback callback)
        {
            if (_AuthState != AuthState.NotAuth)
            {
                Logger.Error("unknown state");
                return;
            }
            _Callback = callback;
            TimerMgr.AddTimer(1000, OnAuth);
        }

        private void OnAuth(object? param)
        {
            if (_Username == "xiao")
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

        public string GetUsername()
        {
            return _Username;
        }

        public void Send(object message)
        {
            _Session.Send(message);
        }

        private void _OnSessionDisconnect(TcpSession session)
        {
        }

        private AuthState _AuthState;
        private readonly string _Username;
        private readonly string _Password;
        private  TcpSession _Session;
        private OnAuthResultCallback? _Callback;
    }
}

