using System;

#pragma warning disable CS8602

namespace Asuna.Foundation.Servers
{
    public sealed class GameServer : ServerBase
    {
        public GameServer(ServerGroupConfig groupConfig, GameServerConfig serverConfig) : base(groupConfig, serverConfig)
        {
            EntityMgr.Init();
        }
        
        protected override void _OnControlMsgStartupStubs(TcpSession session, MsgBase msg)
        {
            var notify = msg as ControlMsgStartupStubsNotify;
            ALogger.LogInfo($"_OnControlMsgStartupStubs stubs count:{notify.Items.Count}");
            foreach (var (key, value) in notify.Items)
            {
                if (value != _ServerConfig.Name)
                {
                    continue;
                }
                var stubType = Type.GetType(key);
                if (stubType == null)
                {
                    ALogger.LogError("unknown type error!");
                    return;
                }
                var stub = EntityMgr.Create(stubType) as ServerStubEntity;
                stub.Init(_OnStubReady);
            }
            ServerStubCaller.Register(notify.Items);
        }

        private void _OnStubReady(string stubName)
        {
            var gm = _ServerToSession[_ServerGroupConfig.GMServer.Name];
            var msg = new ControlMsgStubReadyNotify(stubName);
            gm.SendMsg(msg);
        }
        
    }
}

