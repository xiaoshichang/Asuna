using System;
using AsunaClient.Application.Config;
using AsunaClient.Foundation;
using CodiceApp;
using UnityEngine;


namespace AsunaClient.Application
{
    public enum ApplicationState
    {
        Ready,
        Initializing,
        Running,
        Releasing,
        Released
    }
    
    public partial class ApplicationRoot : MonoBehaviour
    {
        
        private void _InitLogManager()
        {
            XDebug.Init();
        }
        
        
        private void _EnterGameplay()
        {
            //NetworkMgr.ConnectToAsync("127.0.0.1", 50001, OnConnected);
            _GameplayInstance.EntryGameplay();
        }

        private void _CheckConfig()
        {
            if (ApplicationSetting is null)
            {
                throw new Exception("application config is empty");
            }

            if (ApplicationSetting.GameplayAssemblies is null || ApplicationSetting.GameplayAssemblies.Count == 0)
            {
                throw new Exception("GameplayAssemblies is empty");
            }
            
        }
        
        private void _ApplicationStartup()
        {
            _CheckConfig();
            G.SetupConfig(this);
            
            _InitLogManager();
            _InitAssetManager();
            G.SetupCoreManagers(this);
            
            _InitNetwork();
            _InitUIManager();
            _InitEntityManager();
            G.SetupOtherManagers(this);
            
            _InitGameplay();
            G.SetupGameplay(_GameplayInstance);
            
            _EnterGameplay();
        }
        
        void Awake()
        {
            _State = ApplicationState.Ready;
            if (ApplicationSetting == null)
            {
                Debug.LogError("application config is empty");
                return;
            }
            DontDestroyOnLoad(gameObject);
            _State = ApplicationState.Initializing;
            _ApplicationStartup();
        }

        void Update()
        {
            if (_State == ApplicationState.Running)
            {
                TimerMgr.Tick();
                NetworkManager.Tick();
            }

            if (_State == ApplicationState.Initializing)
            {
                // todo: check all managers and systems are initialized
                if (true)
                {
                    _State = ApplicationState.Running;
                }
            }
        }

        private void OnApplicationQuit()
        {
            if (_State == ApplicationState.Ready)
            {
                return;
            }

            _State = ApplicationState.Releasing;
            _ReleaseGameplay();
            G.ResetGameplay();

            _ReleaseEntityManager();
            _ReleaseUIManager();
            _ReleaseNetworkManager();
            G.ResetOtherManagers();
            
            _ReleaseAssetManager();
            G.ResetCoreManagers();
            _State = ApplicationState.Released;
        }

        private ApplicationState _State = ApplicationState.Ready;
        public ApplicationSetting ApplicationSetting;


    }
}

