using System;
using System.Reflection;
using AsunaClient.Application.Gameplay;
using AsunaClient.Foundation;

namespace AsunaClient.Application
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
                    if (type.IsSubclassOf(typeof(Gameplay.GameplayInstance)))
                    {
                        count += 1;
                        entryType = type;
                    }
                }
            }
            
            XDebug.Asset(count == 1);
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
            XDebug.Info("Init Gameplay Ok!");
        }

        private void _ReleaseGameplay()
        {
            _GameplayInstance.Release();
        }

        private GameplayInstance _GameplayInstance;
    }
}