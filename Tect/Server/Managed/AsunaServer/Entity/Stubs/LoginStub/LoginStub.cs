using AsunaServer.Debug;
using AsunaServer.Network;
using AsunaServer.Timer;

namespace AsunaServer.Entity;

public class LoginStub : ServerStubEntity
{
    
    public override void Init()
    {
        TimerMgr.AddTimer(1000, _SomeLogicFinish);
    }
    
    private void _SomeLogicFinish(object? param)
    {
        _OnStubReady();
    }

    [Rpc]
    private void _OnCheckAccountLogin(int num, string message)
    {
        Logger.Info($"num : {num}");
        Logger.Info($"message : {message}");
    }

}