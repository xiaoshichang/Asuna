using System;
using Asuna.Gameplay;
using Asuna.Timer;
using Asuna.Utils;
using UnityEngine;


namespace Asuna.Application
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
            ADebug.Init();
        }
        
        void Awake()
        {
            _State = ApplicationState.Ready;
            if (ApplicationSetting == null)
            {
                return;
            }
            DontDestroyOnLoad(gameObject);
            _State = ApplicationState.Initializing;
            _ApplicationStartup();
        }

        void Update()
        {
            var dt = Time.deltaTime;
            if (_State == ApplicationState.Running)
            {
                PlayerInputManager.Update(dt);
                TimerManager.Update(dt);
                NetworkManager.Update(dt);
                EntityManager.Update(dt);
                GameplayInstance.Update(dt);
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

        private void LateUpdate()
        {
            var dt = Time.deltaTime;
            if (_State == ApplicationState.Running)
            {
                CameraManager.LateUpdate(dt);
            }
        }


        private void _EnterGameplay()
        {
            //NetworkMgr.ConnectToAsync("127.0.0.1", 50001, OnConnected);
            GameplayInstance.EntryGameplay();
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
            _InitInputManager();
            _InitCoroutineManager();
            _InitTimerManager();
            _InitAssetManager();
            G.SetupCoreManagers(this);
            
            _InitCameraManager();
            _InitNetwork();
            _InitUIManager();
            _InitEntityManager();
            G.SetupOtherManagers(this);
            
            _InitGameplay();
            G.SetupGameplay(GameplayInstance);
            
            _EnterGameplay();
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
            _ReleaseCameraManager();
            G.ResetOtherManagers();
            
            _ReleaseAssetManager();
            _ReleaseTimerManager();
            _ReleaseCoroutineManager();
            _ReleaseInputManager();
            G.ResetCoreManagers();
            _State = ApplicationState.Released;
        }

        private ApplicationState _State = ApplicationState.Ready;
        public ApplicationSetting ApplicationSetting;


    }
}

