using AsunaServer.Account;
using AsunaServer.Application;
using AsunaServer.Message;
using AsunaServer.Debug;
using AsunaServer.Network;
using AsunaShared.Message;

namespace AsunaServer.Application;

public partial class GateServer : ServerBase
{
    private void _OnOpenGateNtf(TcpSession session, OpenGateNtf message)
    {
        var gateConfig = _ServerConfig as GateServerConfig;
        if (gateConfig == null)
        {
            Logger.Error("unknown config type!");
            return;
        }
        OuterNetwork.Init(gateConfig.OuterIp, gateConfig.OuterPort, _OnAcceptClientConnection, null, _OnReceiveClientMessage, _OnClientDisconnect);
        Logger.Info($"open gate at {gateConfig.OuterIp} {gateConfig.OuterPort}");
    }

    private void _OnAcceptClientConnection(TcpSession session)
    {
        // todo: remove client if timeout without login
        Logger.Debug($"on client connected! {session.GetConnectionID()}");
    }
    
    private void _OnReceiveClientMessage(TcpSession session, object message)
    {
        if (!_HandleMessage(session, message))
        {
            Logger.Error($"message unhandled! {message.GetType()}");
        }
    }

    private void _OnLoginReq(TcpSession session, LoginReq req)
    {
        var account = new Account.Account(session);
        _Accounts[session] = account;
        account.Auth(req.Username, req.Password, _OnAuthResult);
    }

    private void _OnClientDisconnect(TcpSession session)
    {
        if (!_Accounts.ContainsKey(session))
        {
            Logger.Warning($"should contain session {session}");
            return;
        }

        var account = _Accounts[session];
        Logger.Info($"_OnAccountDisconnect {account.Username} {session.GetConnectionID()}");
        _Accounts.Remove(session);
    }

    private void _OnAuthResult(Account.Account account)
    {
        if (account.GetAuthState() == AuthState.AuthSuccess)
        {
            var rsp = new LoginRsp()
            {
                Username = account.Username,
                RetCode = LoginRetCode.Ok,
            };
            account.Send(rsp);
        }
        else if (account.GetAuthState() == AuthState.AuthFail)
        {
            var rsp = new LoginRsp()
            {
                Username = account.Username,
                RetCode = LoginRetCode.Fail,
            };
            account.Send(rsp);
        }
        else
        {
            Logger.Error("unknown state");
        }
    }

    private readonly Dictionary<TcpSession, Account.Account> _Accounts = new Dictionary<TcpSession, Account.Account>();
}