using AsunaServer.Entity;
using AsunaServer.Foundation.Debug;
using AsunaServer.Network;

namespace AsunaServer.Auth;

public partial class Account
{
    /// <summary>
    /// 玩家从Account中选择一个Avatar登录
    /// </summary>
    [Rpc]
    private void SelectAvatar(Guid avatarID)
    {
        ADebug.Info($"SelectAvatar {avatarID}");
        RpcCaller.CallStub("LoginStub", "AvatarLogin", Proxy, avatarID);
    }

    [Rpc]
    private void OnSelectAvatarResult(AvatarLoginResult result, AvatarProxy proxy)
    {
        ADebug.Info($"OnSelectAvatar {result}");
        _AvatarProxy = proxy;
    }

    private AvatarProxy _AvatarProxy;
}