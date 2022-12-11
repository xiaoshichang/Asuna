using System.Collections;
using System.Collections.Generic;
using AsunaClient.Foundation;
using UnityEngine;


namespace AsunaClient.Application
{
    public partial class ApplicationRoot : MonoBehaviour
    {
        
        private void _InitLogManager()
        {
            XDebug.Init();
        }

        private void _EnterGame()
        {
            //NetworkMgr.ConnectToAsync("127.0.0.1", 50001, OnConnected);
        }
        
        

        private void _ApplicationStartup()
        {
            _InitLogManager();
            _InitAssetManager();
            _InitNetwork();
            _InitUIManager();
            _InitEntityManager();
            
            _InitGMSystem();
            G.Setup(this);
            
            _EnterGame();
        }
        
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _ApplicationStartup();
        }

        void Update()
        {
            TimerMgr.Tick();
            NetworkManager.Tick();
        }

        private void OnApplicationQuit()
        {
            _ReleaseGMSystem();

            _ReleaseEntityManager();
            _ReleaseUIManager();
            _ReleaseNetworkManager();
            _ReleaseAssetManager();
        }


    }
}

