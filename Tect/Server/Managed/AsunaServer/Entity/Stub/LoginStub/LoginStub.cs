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
            _OnAvatarLoginError(proxy, avatarID, AvatarLoginResult.AlreadyLogin);
            return;
        }

        _CreateAvatarOnGameServer(proxy, avatarID);
    }

    private void _OnAvatarLoginError(AccountProxy proxy, Guid avatarID, AvatarLoginResult result)
    {
        switch (result)
        {
            case AvatarLoginResult.AlreadyLogin:
            {
                throw new NotImplementedException();
            }
            default:
            {
                throw new NotImplementedException();
            }
        }
    }

    /// <summary>
    /// 创建 Avatar 实体
    /// </summary>
    private void _CreateAvatarOnGameServer(AccountProxy proxy, Guid avatarID)
    {
        _CreatingAvatar[avatarID] = proxy;
        var avatarType = EntityMgr.GetAvatarType();
        if (avatarType == null)
        {
            ADebug.Error("avatar type not found!");
            return;
        }
        
        EntityMgr.CreateEntityRemote(avatarType, avatarID);
    }

    /// <summary>
    /// Avatar 实体创建完毕回调
    /// </summary>
    [Rpc]
    private void OnAvatarCreated(AvatarProxy proxy)
    {
        ADebug.Assert(_CreatingAvatar.ContainsKey(proxy.AvatarID));
        var accountProxy = _CreatingAvatar[proxy.AvatarID];
        _CreatingAvatar.Remove(proxy.AvatarID);
        _OnlineAvatar[proxy.AvatarID] = proxy;
        RpcCaller.CallProxy(accountProxy, "OnSelectAvatarResult", AvatarLoginResult.OK, proxy);
        
        ADebug.Info($"OnAvatarCreated - Avatar {proxy.AvatarID} created on {proxy.Game}");
    }

    private readonly Dictionary<Guid, AccountProxy> _CreatingAvatar = new();
    private readonly Dictionary<Guid, AvatarProxy> _OnlineAvatar = new();


}