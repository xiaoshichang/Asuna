namespace AsunaServer.Network;

public struct AccountProxy
{
    public Guid AccountID { get; set; }
    public string Gate { get; set; }
}

public struct AvatarProxy
{
    public Guid AvatarID { get; set; }
    public string Gate { get; set; }
}