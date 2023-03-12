namespace AsunaServer.Entity;

public abstract class ServerEntity
{
    protected ServerEntity()
    {
        Guid = new Guid();
    }

    protected ServerEntity(Guid guid)
    {
        Guid = guid;
    }

    public Guid Guid;
}