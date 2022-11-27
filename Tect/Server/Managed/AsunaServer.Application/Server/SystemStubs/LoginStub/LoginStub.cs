using AsunaServer.Foundation.Entity;
using AsunaServer.Foundation.Network;
using AsunaServer.Foundation.Timer;

namespace AsunaServer.Application.Server.SystemStubs.LoginStub;

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