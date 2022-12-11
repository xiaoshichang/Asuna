using System.Collections;
using System.Collections.Generic;
using AsunaClient.Application.Config;
using AsunaClient.Foundation;
using CodiceApp;
using UnityEngine;
using UnityEngine.Serialization;


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
        }

        private void _ApplicationStartup()
        {
            _InitLogManager();
            _InitAssetManager();
            G.SetupCoreManagers(this);
            
            _InitNetwork();
            _InitUIManager();
            _InitEntityManager();
            G.SetupOtherManagers(this);
            
            _InitGMSystem();
            G.SetupSystems(this);
            
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
            _State = ApplicationState.Running;
        }

        void Update()
        {
            if (_State == ApplicationState.Running)
            {
                TimerMgr.Tick();
                NetworkManager.Tick();
            }
        }

        private void OnApplicationQuit()
        {
            if (_State == ApplicationState.Ready)
            {
                return;
            }

            _State = ApplicationState.Releasing;
            _ReleaseGMSystem();
            G.ResetSystems();

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

