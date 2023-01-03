using AsunaServer.Logger;
using AsunaServer.Network;
using AsunaServer.Message;


namespace AsunaServer.Application;


public abstract partial class ServerBase
{
    
    protected virtual void _OnConnect(TcpSession session)
    {
        var message = new InnerPingReq()
        {
            ServerName = _ServerConfig.Name
        };
        session.Send(message);
    }

    protected virtual void _OnInnerPing(TcpSession session, InnerPingReq req)
    {
        if (_ServerToSession.ContainsKey(req.ServerName))
        {
            Logger.Error($"server {req.ServerName} exist!");
            return;
        }
        _ServerToSession[req.ServerName] = session;
        var pong = new InnerPongRsp()
        {
            ServerName = _ServerConfig.Name
        };
        session.Send(pong);
    }

    protected virtual void _OnInnerPong(TcpSession session, InnerPongRsp rsp)
    {
        if (_ServerToSession.ContainsKey(rsp.ServerName))
        {
            Logger.Error($"server {rsp.ServerName} exist!");
            return;
        }
        _ServerToSession[rsp.ServerName] = session;
    }
    
    protected readonly Dictionary<string, TcpSession> _ServerToSession = new();
    
}