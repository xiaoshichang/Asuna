﻿using System;
using System.Reflection;
using Asuna.Foundation.Debug;
using Asuna.Gameplay;

namespace Asuna.Application
{
    public partial class ApplicationRoot
    {
        private Type _SearchGameplayEntry()
        {
            var count = 0;
            Type entryType = null;
            foreach (var assemblyName in ApplicationSetting.GameplayAssemblies)
            {
                var assembly = Assembly.Load(assemblyName);
                if (assembly == null)
                {
                    throw new Exception($"cannot load assembly {assemblyName}");
                }
                
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(GameplayInstance)))
                    {
                        count += 1;
                        entryType = type;
                    }
                }
            }
            
            ADebug.Assert(count == 1);
            return entryType;
        }
        
        private void _InitGameplay()
        {
            var param = new GameplayInitParam()
            {
                ApplicationRoot = this,
                GameplayAssemblies = ApplicationSetting.GameplayAssemblies
            };
            var entryType = _SearchGameplayEntry();
            GameplayInstance = Activator.CreateInstance(entryType) as GameplayInstance;
            if (GameplayInstance == null)
            {
                throw new Exception("cannot create gameplay entry type!");
            }
            GameplayInstance.Init(param);
            ADebug.Info("Init Gameplay Ok!");
        }

        private void _ReleaseGameplay()
        {
            GameplayInstance.Release();
            ADebug.Info("Release Gameplay Ok!");
        }

        public GameplayInstance GameplayInstance;
    }
}