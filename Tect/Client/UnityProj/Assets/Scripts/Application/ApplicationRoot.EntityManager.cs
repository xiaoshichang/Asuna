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
            EntityManager.Init(null);
            XDebug.Info("Init EntityManager Ok!");
        }

        private void _ReleaseEntityManager()
        {
            EntityManager.Release();
            XDebug.Info("Release EntityManager Ok!");
        }

        public readonly EntityManager EntityManager = new EntityManager();

    }
}