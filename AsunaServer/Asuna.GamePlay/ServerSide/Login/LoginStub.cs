using Asuna.Foundation;
using Asuna.Foundation.Network.Rpc;

namespace Asuna.GamePlay;

public class LoginStub : ServerStubEntity
{
    [Rpc]
    public void Login(string user, string password)
    {
    }
}