using AsunaServer.Application;
using AsunaServer.Message;
using AsunaServer.Debug;

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
        Logger.Info($"OnStubReady {GetType().Name}");
        var ntf = new StubReadyNtf()
        {
            StubName = GetType().Name
        };
        G.GM.Send(ntf);
    }

}