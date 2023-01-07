﻿using System.Collections.Generic;
using Asuna.Application.GM;
using Asuna.Utils;

namespace Asuna.Application
{
    public partial class GameplayInstance
    {
        private void _InitGMSystem(GameplayInitParam param)
        {
            GMSystem = new GMSystem();
            GMSystem.Init(param.GameplayAssemblies);
            param.ApplicationRoot.gameObject.AddComponent<GMTerminal>();
            ADebug.Info($"Init GM system Ok! {GMSystem.GetCommandsCount()} commands found!");
        }

        private void _ReleaseGMSystem()
        {
            GMSystem.Release();
            GMSystem = null;
            ADebug.Info("Release GM system Ok!");
        }

        public GMSystem GMSystem;
    }
}