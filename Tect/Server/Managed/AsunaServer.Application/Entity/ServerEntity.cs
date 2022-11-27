namespace AsunaServer.Foundation.Entity;

public abstract class ServerEntity
{
    protected ServerEntity()
    {
        Guid = new Guid();
        EntityMgr.AddEntity(Guid, this);
    }

    protected ServerEntity(Guid guid)
    {
        Guid = guid;
        EntityMgr.AddEntity(Guid, this);
    }
    
    public Guid Guid;
}