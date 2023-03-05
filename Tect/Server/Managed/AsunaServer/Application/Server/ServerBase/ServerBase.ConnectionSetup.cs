using AsunaServer.Foundation.Debug;
using AsunaServer.Network;
using AsunaServer.Message;


namespace AsunaServer.Application;


public abstract partial class ServerBase
{
    protected virtual void _OnNodeAccept(TcpSession session)
    {
    }
    
    /// <summary>
    /// 由主动connect方发起ping
    /// </summary>
    protected virtual void _OnNodeConnect(TcpSession session)
    {
        var message = new InnerPingReq()
        {
            ServerName = G.ServerConfig.Name
        };
        session.Send(message);
    }

    protected virtual void _OnNodeDisconnect(TcpSession session)
    {
        if (!Sessions.UnRegisterSession(session))
        {
            ADebug.Error("UnRegisterSession fail! unknown state.");
            return;
        }
    }

    protected virtual void _OnInnerPing(TcpSession session, object message)
    {
        var req = message as InnerPingReq;
        if (req == null)
        {
            throw new ArgumentException();
        }
        if (!Sessions.RegisterSession(req.ServerName, session))
        {
            ADebug.Error($"Register Session fail!");
            return;
        }
        var pong = new InnerPongRsp()
        {
            ServerName = G.ServerConfig.Name
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
        if (!Sessions.RegisterSession(rsp.ServerName, session))
        {
            ADebug.Error($"Register Session fail!");
            return;
        }
    }
    
    

}