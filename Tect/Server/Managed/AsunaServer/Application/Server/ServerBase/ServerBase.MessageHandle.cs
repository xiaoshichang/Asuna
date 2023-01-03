using AsunaServer.Message;
using AsunaServer.Logger;
using AsunaServer.Network;

#pragma warning disable CS8604

namespace AsunaServer.Application
{
    public abstract partial class ServerBase
    {
        /// <summary>
        /// callback when server receive a message
        /// </summary>
        /// <param name="session"> the session </param>
        /// <param name="message"> the message </param>
        private void _OnReceiveMessage(TcpSession session, object message)
        {
            if (!_HandleMessage(session, message))
            {
                Logger.Error($"message unhandled! {message.GetType()}");
            }
        }

        protected virtual bool _HandleMessage(TcpSession session, object message)
        {
            var type = message.GetType();
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

