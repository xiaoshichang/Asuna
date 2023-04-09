using AsunaServer.Application;
using AsunaServer.Network;

namespace AsunaServer.Entity;

public partial class AvatarBase : ServerEntity
{
    protected void _InitAvatarProxy()
    {
        AvatarProxy = new AvatarProxy()
        {
            AvatarID = Guid,
            Game = G.ServerConfig.Name
        };
    }
    
    
    private void _ReportAvatarCreated()
    {
        RpcCaller.CallStub("LoginStub", "OnAvatarCreated", AvatarProxy);
    }


    public AvatarProxy AvatarProxy;
}