using System;
using System.Collections;
using System.Collections.Generic;
using Asuna.Foundation;
using UnityEngine;
using Logger = Asuna.Foundation.Logger;

namespace Asuna.Application
{
    public class ApplicationRoot : MonoBehaviour
    {
        private void _InitGMManager()
        {
            var assemblyList = new List<string>()
            {
                "Asuna.GamePlay",
                "Asuna.GamePlayCore",
                "Asuna.UI"
            };
            GMManager.Init(assemblyList);
            gameObject.AddComponent<GMTerminal>();
        }

        private void _InitLogManager()
        {
            Logger.Init();
        }

        private void _InitNetworkManager()
        {
            NetworkMgr.Init();
        }
        
        void Awake()
        {
            _InitGMManager();
            _InitLogManager();
            _InitNetworkManager();
        }
        
        void Start()
        {
            NetworkMgr.ConnectToServer("127.0.0.1", 40001, ConnectCallback);
            NetworkMgr.OnReceiveMsg = OnReceivePackage;
        }

        void Update()
        {
            NetworkMgr.Tick();
            TimerMgr.Tick();
        }
        
        private void ConnectCallback(Exception e)
        {
            if (e != null)
            {
                Debug.Log(e.Message);
                return;
            }
            Debug.Log("Connect to server ok!");
        }

        private void OnReceivePackage(PackageBase package)
        {
        }
    }
}

