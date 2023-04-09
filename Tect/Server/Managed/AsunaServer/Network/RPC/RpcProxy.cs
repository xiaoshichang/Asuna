namespace AsunaServer.Network;

[Rpc]
public struct AccountProxy
{
    public Guid AccountID { get; set; }
    public string Gate { get; set; }
}

[Rpc]
public struct AvatarProxy
{
    public Guid AvatarID { get; set; }
    public string Game { get; set; }
}