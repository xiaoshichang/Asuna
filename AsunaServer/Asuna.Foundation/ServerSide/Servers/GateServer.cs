
namespace Asuna.Foundation.Servers
{
    public sealed class GateServer : ServerBase
    {
        public GateServer(ServerGroupConfig groupConfig, GateServerConfig serverConfig) : base(groupConfig, serverConfig)
        {
        }
        
        protected override void _OnControlMsgHandShakeRsp(TcpSession session, MsgBase msg)
        {
            base._OnControlMsgHandShakeRsp(session, msg);
            if (_ServerToSession.Count == 1 + _ServerGroupConfig.GameServers.Count)
            {
                ALogger.LogInfo("All games connected!");
                var gm = _ServerToSession[_ServerGroupConfig.GMServer.Name];
                var notify = new ControlMsgGamesConnectedNotify();
                gm.SendMsg(notify);
            }
        }
    }
}

