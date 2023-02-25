using AsunaServer.Debug;
using AsunaServer.Network;
using AsunaServer.Message;


namespace AsunaServer.Application;


public abstract partial class ServerBase
{
    protected virtual void _OnNodeAccept(TcpSession session)
    {
    }
    
    protected virtual void _OnNodeConnect(TcpSession session)
    {
        var message = new InnerPingReq()
        {
            ServerName = _ServerConfig.Name
        };
        session.Send(message);
    }

    protected virtual void _OnNodeDisconnect(TcpSession session)
    {
        if (!_SessionToServer.TryGetValue(session, out var serverName))
        {
            Logger.Error("unknown state");
            return;
        }

        _ServerToSession.Remove(serverName);
        _SessionToServer.Remove(session);
    }

    protected virtual void _OnInnerPing(TcpSession session, object message)
    {
        var req = message as InnerPingReq;
        if (req == null)
        {
            throw new ArgumentException();
        }
        if (_ServerToSession.ContainsKey(req.ServerName))
        {
            Logger.Error($"server {req.ServerName} exist!");
            return;
        }
        _ServerToSession[req.ServerName] = session;
        _SessionToServer[session] = req.ServerName;
        var pong = new InnerPongRsp()
        {
            ServerName = _ServerConfig.Name
        };
        session.Send(pong);
    }

    protected virtual void _OnInnerPong(TcpSession session, object message)
    {
        var rsp = message as InnerPongRsp;
        if (rsp == null)
        {
            throw new ArgumentException();
        }
        if (_ServerToSession.ContainsKey(rsp.ServerName))
        {
            Logger.Error($"server {rsp.ServerName} exist!");
            return;
        }
        _ServerToSession[rsp.ServerName] = session;
        _SessionToServer[session] = rsp.ServerName;
    }
    
    protected readonly Dictionary<string, TcpSession> _ServerToSession = new();
    protected readonly Dictionary<TcpSession, string> _SessionToServer = new();

}