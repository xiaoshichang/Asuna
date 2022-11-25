using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Network;

#pragma warning disable CS8604

namespace AsunaServer.Application.Server
{
    public abstract partial class ServerBase
    {
        /// <summary>
        /// callback when server receive a message
        /// </summary>
        /// <param name="session"> the session </param>
        /// <param name="message"> the message </param>
        /// <param name="type"> the message type </param>
        private void _OnReceiveMessage(TcpSession session, object message, Type type)
        {
            if (!_HandleMessage(session, message, type))
            {
                Logger.Error($"message unhandled! {type}");
            }
        }

        protected virtual bool _HandleMessage(TcpSession session, object message, Type type)
        {
            if (type == typeof(InnerPingReq))
            {
                var req = message as InnerPingReq;
                _OnInnerPing(session, req);
                return true;
            }
            else if (type == typeof(InnerPongRsp))
            {
                var rsp = message as InnerPongRsp;
                _OnInnerPong(session, rsp);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

