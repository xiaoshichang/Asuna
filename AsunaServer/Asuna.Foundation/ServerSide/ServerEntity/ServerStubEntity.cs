namespace Asuna.Foundation;



public delegate void StubReadyCallback(string stubName);

public abstract class ServerStubEntity : ServerEntity
{
    public void Init(StubReadyCallback onReady)
    {
        onReady.Invoke(GetType().Name);
        Logger.LogInfo($"{GetType().Name} is ready!");
    }
}