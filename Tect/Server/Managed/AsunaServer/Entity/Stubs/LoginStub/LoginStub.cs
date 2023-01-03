using AsunaServer.Entity;
using AsunaServer.Network;
using AsunaServer.Timer;

namespace AsunaServer.Application.SystemStubs.LoginStub;

public class LoginStub : ServerStubEntity
{
    
    public override void Init()
    {
        TimerMgr.AddTimer(1000, _SomeLogicFinish);
    }
    
    private void _SomeLogicFinish(object? param)
    {
        OnStubReady();
    }


}