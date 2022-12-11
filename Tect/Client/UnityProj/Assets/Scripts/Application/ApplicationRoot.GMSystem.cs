using System.Collections.Generic;
using AsunaClient.Application.GM;
using AsunaClient.Foundation;

namespace AsunaClient.Application
{
    public partial class ApplicationRoot
    {
        private void _InitGMSystem()
        {
            var assemblyList = new List<string>()
            {
            };
            GmSystem.Init(assemblyList);
            gameObject.AddComponent<GMTerminal>();
            XDebug.Info("Init GM Ok!");
        }

        private void _ReleaseGMSystem()
        {
            
        }

        public GMSystem GmSystem = new GMSystem();
    }
}