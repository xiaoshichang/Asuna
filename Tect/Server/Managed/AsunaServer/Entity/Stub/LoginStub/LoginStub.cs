using AsunaServer.Foundation.Debug;
using AsunaServer.Network;
using AsunaServer.Timer;

namespace AsunaServer.Entity;

[Rpc]
public enum AvatarLoginResult
{
    OK,
    AlreadyLogin,
}

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
    private void AvatarLogin(AccountProxy proxy, Guid avatarID)
    {
        ADebug.Info($"AvatarLogin {proxy.AccountID} {proxy.Gate} {avatarID}");
        if (_OnlineAvatar.ContainsKey(avatarID) || _CreatingAvatar.ContainsKey(avatarID))
        {
            _OnAvatarLogin(proxy, avatarID, AvatarLoginResult.AlreadyLogin);
            return;
        }

        _OnAvatarLogin(proxy, avatarID, AvatarLoginResult.OK);
    }

    private void _OnAvatarLogin(AccountProxy proxy, Guid avatarID, AvatarLoginResult result)
    {
        switch (result)
        {
            case AvatarLoginResult.OK:
            {
                _CreatingAvatar[avatarID] = proxy;
                RpcCaller.CallProxy(proxy, "OnSelectAvatarResult", result);
                break;
            }
            case AvatarLoginResult.AlreadyLogin:
            {
                RpcCaller.CallProxy(proxy, "OnSelectAvatarResult", result);
                break;
            }
            default:
            {
                throw new NotImplementedException();
            }
        }
    }

    private readonly Dictionary<Guid, AccountProxy> _CreatingAvatar = new();
    private readonly Dictionary<Guid, AccountProxy> _OnlineAvatar = new();


}