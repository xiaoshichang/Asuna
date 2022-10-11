namespace Asuna.Foundation
{
    public class Entity
    {
        protected Entity()
        {
#if AsunaSide_Client
            ALogger.Error("Client entity");
#endif
            
#if AsunaSide_Server
            Logger.LogError("Server entity");
#endif
        }
    }
}