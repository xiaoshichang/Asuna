using AsunaServer.Entity;
using AsunaServer.Foundation.Debug;
using AsunaServer.Message;
using AsunaServer.Network;

namespace AsunaServer.Application;

public partial class GameServer : ServerBase
{
    protected void _OnCreateServerEntity(TcpSession session, object message)
    {
        var ntf = message as CreateServerEntityNtf;
        if (ntf == null)
        {
            ADebug.Error("unknown");
            return;
        }

        var t = EntityMgr.GetServerEntityTypeByIndex(ntf.TypeIndex);
        if (t == null)
        {
            ADebug.Error("type not found");
            return;
        }

        if (!RpcHelper.ConvertRpcArgs(ntf.ArgsCount, ntf.Args, ntf.ArgsTypeIndex, out var args))
        {
            ADebug.Error("Convert args fail");
            return;
        }
        
        EntityMgr.CreateEntityLocal(t, args);
    }
}