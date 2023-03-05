using Asuna.Entity;
using Asuna.Foundation.Debug;

namespace Asuna.Application
{
    public partial class ApplicationRoot
    {
        
        private void _InitEntityManager()
        {
            EntityManager = new EntityManager();
            EntityManager.Init(null);
            ADebug.Info("Init EntityManager Ok!");
        }

        private void _ReleaseEntityManager()
        {
            EntityManager.Release();
            EntityManager = null;
            ADebug.Info("Release EntityManager Ok!");
        }

        public EntityManager EntityManager;

    }
}