namespace Asuna.Foundation
{
    public class Entity
    {
        protected Entity()
        {
#if AsunaSide_Client
            Logger.Error("Client entity");
#endif
            
#if AsunaSide_Server
            Logger.LogError("Server entity");
#endif
        }
    }
}