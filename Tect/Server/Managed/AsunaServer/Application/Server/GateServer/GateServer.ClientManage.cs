using AsunaServer.Auth;
using AsunaServer.Message;
using AsunaServer.Foundation.Debug;
using AsunaServer.Network;
using AsunaShared.Message;

namespace AsunaServer.Application;

public partial class GateServer : ServerBase
{
    private void _OnOpenGateNtf(TcpSession session, object message)
    {
        var ntf = message as OpenGateNtf;
        if (ntf == null)
        {
            throw new ArgumentException();
        }
        RpcCaller.StubsDistributeTable = ntf.StubsDistributeTable;
        var gateConfig = G.ServerConfig as GateServerConfig;
        if (gateConfig == null)
        {
            ADebug.Error("unknown config type!");
            return;
        }
        OuterNetwork.Init(gateConfig.OuterIp, gateConfig.OuterPort, _OnAcceptClientConnection, null, _OnReceiveClientMessage, _OnClientDisconnect);
        ADebug.Info($"open gate at {gateConfig.OuterIp} {gateConfig.OuterPort}");
    }

    private void _OnAcceptClientConnection(TcpSession session)
    {
        // todo: remove client if timeout without login
        ADebug.Info($"on client connected! {session.GetConnectionID()}");
    }
    
    private void _OnReceiveClientMessage(TcpSession session, object message)
    {
        var handler = _GetMessageHandler(message.GetType());
        handler?.Invoke(session, message);
    }

    private void _OnLoginReq(TcpSession session, object message)
    {
        var req = message as AuthReq;
        if (req == null)
        {
            throw new ArgumentException();
        }
        var account = new Account(req, session, _OnAuthResult);
        _SessionToAccount[session] = account;
        _GuidToAccount[account.Guid] = account;
        account.Auth();
    }

    private void _OnClientDisconnect(TcpSession session)
    {
        if (!_SessionToAccount.ContainsKey(session))
        {
            ADebug.Warning($"should contain session {session}");
            return;
        }

        var account = _SessionToAccount[session];
        ADebug.Info($"_OnAccountDisconnect {account.Username} {session.GetConnectionID()}");
        _SessionToAccount.Remove(session);
        _GuidToAccount.Remove(account.Guid);
    }

    private void _OnAuthResult(Account account)
    {
        ADebug.Assert(account.AuthState == AuthState.Finish);
        if (account.AuthResult != AuthRetCode.Ok)
        {
            if (_GuidToAccount.ContainsKey(account.Guid))
            {
                _GuidToAccount.Remove(account.Guid);
                _SessionToAccount.Remove(account.Session);
            }
        }
    }

    private readonly Dictionary<TcpSession, Account> _SessionToAccount = new();
    private readonly Dictionary<Guid, Account> _GuidToAccount = new();
}