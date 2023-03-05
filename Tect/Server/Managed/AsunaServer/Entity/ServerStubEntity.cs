using AsunaServer.Message;
using AsunaServer.Foundation.Debug;
using AsunaServer.Network;

#pragma warning disable CS8602

namespace AsunaServer.Entity;

public abstract class ServerStubEntity : ServerEntity
{

    /// <summary>
    /// 初始化。
    /// 由GameServer驱动。
    /// </summary>
    public abstract void Init();
        
    
    /// <summary>
    /// 准备就绪。
    /// 初始化完毕后主动调用。
    /// </summary>
    protected void _OnStubReady()
    {
        ADebug.Info($"OnStubReady {GetType().Name}");
        var ntf = new StubReadyNtf()
        {
            StubName = GetType().Name
        };
        Sessions.GM.Send(ntf);
    }

}