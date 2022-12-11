﻿using System.Collections.Generic;
using AsunaClient.Application.GM;
using AsunaClient.Foundation;

namespace AsunaClient.Application
{
    public partial class ApplicationRoot
    {
        private void _InitGMSystem()
        {
            GMSystem = new GMSystem();
            var assemblyList = new List<string>()
            {
            };
            GMSystem.Init(assemblyList);
            gameObject.AddComponent<GMTerminal>();
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