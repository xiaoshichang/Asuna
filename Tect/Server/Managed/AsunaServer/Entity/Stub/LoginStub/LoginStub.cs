using AsunaServer.Foundation.Debug;
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
    private void AvatarLogin(Guid accountID, Guid avatarID)
    {
    }
    

}