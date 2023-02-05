using AsunaServer.Message;
using AsunaServer.Debug;
using Google.Protobuf;

#pragma warning disable CS8602

namespace AsunaServer.Entity;

public delegate void NetworkDelegate(IMessage message);

public abstract class ServerStubEntity : ServerEntity
{

    /// <summary>
    /// 初始化。
    /// 由GameServer驱动。
    /// </summary>
    public abstract void Init();
        
    
    /// <summary>
    /// 准备继续。
    /// 初始化完毕后主动调用。
    /// </summary>
    protected void _OnStubReady()
    {
        Logger.Info($"OnStubReady {GetType().Name}");
        var ntf = new StubReadyNtf()
        {
            StubName = GetType().Name
        };
        _GMDelegate.Invoke(ntf);
    }

    public void SetCallGMDelegate(NetworkDelegate gm)
    {
        _GMDelegate = gm;
    }

    private NetworkDelegate? _GMDelegate;

}