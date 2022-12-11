using System.Collections;
using AsunaClient.Foundation.Entity;
using AsunaClient.Foundation;
using UnityEngine;

namespace AsunaClient.Application
{
    public partial class ApplicationRoot
    {
        
        private void _InitEntityManager()
        {
            EntityManager.Init();
            XDebug.Info("Init EntityManager Ok!");
        }

        private void _ReleaseEntityManager()
        {
            EntityManager.Release();
        }

        public readonly EntityManager EntityManager = new EntityManager();

    }
}