using AsunaServer.Foundation.Debug;

namespace AsunaServer.Entity;

/// <summary>
/// 描述与Gameplay无关的Avatar基础功能
/// </summary>
public partial class AvatarBase : ServerEntity
{
    protected AvatarBase(Guid guid) : base(guid)
    {
        _InitAvatarProxy();
        _ReportAvatarCreated();
        ADebug.Info($"Avatar Created {guid}");
    }
    
    
}