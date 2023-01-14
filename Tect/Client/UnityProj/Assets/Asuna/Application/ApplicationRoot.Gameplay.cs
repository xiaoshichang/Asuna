using System;
using System.Reflection;
using Asuna.Gameplay;
using Asuna.Utils;

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
            _GameplayInstance = Activator.CreateInstance(entryType) as GameplayInstance;
            if (_GameplayInstance == null)
            {
                throw new Exception("cannot create gameplay entry type!");
            }
            _GameplayInstance.Init(param);
            ADebug.Info("Init Gameplay Ok!");
        }

        private void _ReleaseGameplay()
        {
            _GameplayInstance.Release();
            ADebug.Info("Release Gameplay Ok!");
        }

        private GameplayInstance _GameplayInstance;
    }
}