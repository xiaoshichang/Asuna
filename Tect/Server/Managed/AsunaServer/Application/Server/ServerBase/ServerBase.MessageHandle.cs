using AsunaServer.Debug;
using AsunaServer.Message;
using AsunaServer.Network;

#pragma warning disable CS8604

namespace AsunaServer.Application
{
    public delegate void MessageHandler(TcpSession session, object message);
    
    public abstract partial class ServerBase
    {
        protected void _RegisterMessageHandler(Type type, MessageHandler handler)
        {
            if (_Handlers.ContainsKey(type))
            {
                Logger.Error("_RegisterMessageHandler duplicated");
                return;
            }

            _Handlers[type] = handler;
        }

        protected MessageHandler? _GetMessageHandler(Type type)
        {
            if (_Handlers.TryGetValue(type, out var handler))
            {
                return handler;
            }
            return null;
        }

        /// <summary>
        /// callback when server receive a message
        /// </summary>
        /// <param name="session"> the session </param>
        /// <param name="message"> the message </param>
        private void _OnNodeReceiveMessage(TcpSession session, object message)
        {
            var handler = _GetMessageHandler(message.GetType());
            handler?.Invoke(session, message);
        }
        
        protected virtual void _RegisterMessageHandlers()
        {
            _RegisterMessageHandler(typeof(InnerPongRsp), _OnInnerPong);
            _RegisterMessageHandler(typeof(InnerPingReq), _OnInnerPing);
        }

        protected readonly Dictionary<Type, MessageHandler> _Handlers = new();
    }
}

