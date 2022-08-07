
using Asuna.Foundation;

namespace Asuna.Application
{
    public sealed class DBServer : ServerBase
    {
        public DBServer(ServerGroupConfig groupConfig, DBServerConfig serverConfig) : base(groupConfig, serverConfig)
        {
        }
        
        protected override void _OnControlMsgHandShakeRsp(TcpSession session, MsgBase msg)
        {
            base._OnControlMsgHandShakeRsp(session, msg);
            if (_ServerToSession.Count == 1 + _ServerGroupConfig.GameServers.Count)
            {
                Logger.LogInfo("All games connected!");
                var gm = _ServerToSession[_ServerGroupConfig.GMServer.Name];
                var notify = new ControlMsgGamesConnectedNotify();
                gm.SendMsg(notify);
            }
        }

    }
}

