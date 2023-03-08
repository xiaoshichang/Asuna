using AsunaServer.Foundation.Debug;
using AsunaServer.Network;

namespace AsunaServer.Auth;

public partial class Account
{
    [Rpc]
    private void SelectAvatar(Guid avatarID)
    {
        ADebug.Info($"SelectAvatar {avatarID}");
    }
}