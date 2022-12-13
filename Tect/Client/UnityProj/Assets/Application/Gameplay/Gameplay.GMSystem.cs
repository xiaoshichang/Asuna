using System.Collections.Generic;
using AsunaClient.Application.GM;
using AsunaClient.Foundation;

namespace AsunaClient.Application.Gameplay
{
    public partial class GameplayInstance
    {
        private void _InitGMSystem(GameplayInitParam param)
        {
            GMSystem = new GMSystem();
            GMSystem.Init(param.GameplayAssemblies);
            param.ApplicationRoot.gameObject.AddComponent<GMTerminal>();
            XDebug.Info("Init GM system Ok!");
        }

        private void _ReleaseGMSystem()
        {
            GMSystem.Release();
            GMSystem = null;
            XDebug.Info("Release GM system Ok!");
        }

        public GMSystem GMSystem;
    }
}