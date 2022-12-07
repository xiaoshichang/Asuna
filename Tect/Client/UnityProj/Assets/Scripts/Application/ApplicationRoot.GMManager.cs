using System.Collections.Generic;
using AsunaClient.Application.GM;
using AsunaClient.Foundation;

namespace AsunaClient.Application
{
    public partial class ApplicationRoot
    {
        private void _InitGMManager()
        {
            var assemblyList = new List<string>()
            {
            };
            GMManager.Init(assemblyList);
            gameObject.AddComponent<GMTerminal>();
            XDebug.Info("Init GM Ok!");
        }

        private void _ReleaseGMManager()
        {
            
        }

        public GMManager GMManager = new GMManager();
    }
}