using System;
using System.Collections.Generic;
using System.Reflection;
using Asuna.Application;
using Asuna.Gameplay;
using Asuna.Utils;

namespace Demo
{
    public class DemoGameplayInstance : GameplayInstance
    {
        public override void Update(float dt)
        {
            _CurrentRunningDemo?.Tick(dt);
        }

        public override void EntryGameplay()
        {
            _CollectAllDemos();
            G.UIManager.ShowPage(nameof(DemoMainPage), null);
        }

        public void EnterDemo(string demoBtn)
        {
            if (_CurrentRunningDemo != null)
            {
                ExitCurrentRunningDemo();
            }

            _CurrentRunningDemo = _AllDemos[demoBtn];
            ADebug.Info($"enter demo: {_CurrentRunningDemo.GetDemoName()}");
            _CurrentRunningDemo.InitDemo();

        }

        public void ExitCurrentRunningDemo()
        {
            if (_CurrentRunningDemo == null)
            {
                return;
            }
            ADebug.Info($"exit current demo: {_CurrentRunningDemo.GetDemoName()}");
            _CurrentRunningDemo.ReleaseDemo();
            _CurrentRunningDemo = null;
        }

        private void _CollectAllDemos()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(DemoCaseBase)))
                {
                    var demo = Activator.CreateInstance(type) as DemoCaseBase;
                    if (demo == null)
                    {
                        ADebug.Warning("unknown error");
                        continue;
                    }
                    _AllDemos[demo.GetDemoName()] = demo;
                }
            }
            ADebug.Info($"{_AllDemos.Count} DemoCase found!");
        }

        public Dictionary<string, DemoCaseBase> GetAllDemos()
        {
            return _AllDemos;
        }

        public DemoCaseBase GetCurrentRunningDemo()
        {
            return _CurrentRunningDemo;
        }

        private readonly Dictionary<string, DemoCaseBase> _AllDemos = new Dictionary<string, DemoCaseBase>();
        private DemoCaseBase _CurrentRunningDemo;

    }
}