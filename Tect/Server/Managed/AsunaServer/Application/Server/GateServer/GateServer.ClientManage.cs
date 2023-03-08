using System.Reflection;
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
    
    private static bool _ConvertArgs(AccountRpc rpc, out object[] args)
    {
        args = new object[rpc.ArgsCount];
        for (var i = 0; i < rpc.ArgsCount; i++)
        {
            var str = rpc.Args[i].ToStringUtf8();
            var type = RpcTable.GetTypeByIndex(rpc.ArgsTypeIndex[i]);
            var obj = System.Text.Json.JsonSerializer.Deserialize(str, type);
            
            if (obj == null)
            {
                ADebug.Error($"_ConvertArgs {i}-th arg is null, do not supported.");
                return false;
            }
            else
            {
                args[i] = obj;
            }
        }
        return true;
    }
    
    private static bool _CheckBeforeInvoke(MethodInfo method, AccountRpc rpc)
    {
        var parameterCount = method.GetParameters().Length;
        if (parameterCount != rpc.ArgsCount)
        {
            ADebug.Error($"_OnRpcCall method {method} arg count not match. {parameterCount} != {rpc.ArgsCount}");
            return false;
        }
        return true;
    }
    
    public void _OnAccountRpc(TcpSession session, object ntf)
    {
        var rpc = ntf as AccountRpc;
        if (rpc == null)
        {
            ADebug.Error("_OnAccountRpc unknown error");
            return;
        }

        var accountID = rpc.Guid.ToGuid();
        if (!_GuidToAccount.TryGetValue(accountID, out var account))
        {
            ADebug.Error($"_OnAccountRpc account not found. {accountID}");
            return;
        }
        
        var method = RpcTable.GetMethodByIndex(rpc.Method);
        if (method == null)
        {
            ADebug.Error($"method not found {rpc.Method}");
            return;
        }
        if (!_ConvertArgs(rpc, out var args))
        {
            return;
        }
        if (!_CheckBeforeInvoke(method, rpc))
        {
            return;
        }
        method.Invoke(account, args);
    }

    private readonly Dictionary<TcpSession, Account> _SessionToAccount = new();
    private readonly Dictionary<Guid, Account> _GuidToAccount = new();
}