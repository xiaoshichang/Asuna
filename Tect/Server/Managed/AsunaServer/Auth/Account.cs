using AsunaServer.Application;
using AsunaServer.Foundation.Debug;
using AsunaServer.Network;
using AsunaServer.Timer;
using AsunaShared.Message;

namespace AsunaServer.Auth
{
    public enum AuthState
    {
        Ready,
        FindAccount,
        Finish,
    }

    public delegate void AuthCallback(Account account);
    
    public partial class Account
    {
        public Account(AuthReq req, TcpSession session, AuthCallback callback)
        {
            Guid = Guid.NewGuid();
            Proxy = new AccountProxy()
            {
                AccountID = Guid,
                Gate = G.ServerConfig.Name
            };
            _Request = req;
            _Session = session;
            _Callback = callback;
            _AuthState = AuthState.Ready;
        }

        public void Auth()
        {
            if (_AuthState != AuthState.Ready)
            {
                ADebug.Error("unknown state");
                return;
            }
            
            ADebug.Info($"Account Auth {_Request.Username}, {_Request.Password}");

            _AuthState = AuthState.FindAccount;
            TimerMgr.AddTimer(1000, OnFindAccountFromDB);
        }

        /// <summary>
        /// 从DB中获取账户相关信息
        /// </summary>
        private void OnFindAccountFromDB(object? param)
        {
            _AuthState = AuthState.Finish;
            _AuthResult = AuthRetCode.Ok;

            var data = new AccountData
            {
                Guid = Guid.ToProto(),
                Username = _Request.Username
            };
            var avatar1 = Guid.NewGuid();
            var avatar2 = Guid.NewGuid();
            data.AvatarList.Add(avatar1.ToProto());
            data.AvatarList.Add(avatar2.ToProto());
            var rsp = new AuthRsp()
            {
                Ret = AuthRetCode.Ok,
                Data = data
            };
            _Session.Send(rsp);
            
            _Callback?.Invoke(this);
            _Callback = null;
        }

        public Guid Guid;
        public AccountProxy Proxy;

        private readonly AuthReq _Request;
        public string Username => _Request.Username;
        private AuthCallback? _Callback;
        
        private readonly TcpSession _Session;
        public TcpSession Session => _Session;

        private AuthState _AuthState;
        public AuthState AuthState => _AuthState;
        
        private AuthRetCode _AuthResult;
        public AuthRetCode AuthResult => _AuthResult;

    }
}

