using AsunaClient.Foundation;
using AsunaClient.Foundation.Entity;

namespace AsunaClient.Application
{
    public partial class ApplicationRoot
    {
        
        private void _InitEntityManager()
        {
            EntityManager = new EntityManager();
            EntityManager.Init(null);
            XDebug.Info("Init EntityManager Ok!");
        }

        private void _ReleaseEntityManager()
        {
            EntityManager.Release();
            EntityManager = null;
            XDebug.Info("Release EntityManager Ok!");
        }

        public EntityManager EntityManager;

    }
}