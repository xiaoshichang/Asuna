using System;
using System.Collections;
using System.Collections.Generic;
using Asuna.Foundation;
using Asuna.Foundation.Network;
using UnityEngine;

namespace Asuna.Application
{
    public class ApplicationRoot : MonoBehaviour
    {
        private IEnumerator _InitGMManagerCo()
        {
            var assemblyList = new List<string>()
            {
                "Asuna.GamePlay",
                "Asuna.GamePlay.UI"
            };
            GMManager.Init(assemblyList);
            gameObject.AddComponent<GMTerminal>();
            yield return null;
        }

        private IEnumerator _InitLogManagerCo()
        {
            ALogger.Init();
            yield return null;
        }

        private IEnumerator _InitUIManagerCo()
        {
            UIManager.Init();
            yield return null;
        }

        private IEnumerator _InitNetworkManagerCo()
        {
            NetworkMgr.Init();
            yield return null;
        }

        private IEnumerator _EnterGameCo()
        {
            NetworkMgr.ConnectToServer("127.0.0.1", 40001, ConnectCallback);
            NetworkMgr.OnReceiveMsg = OnReceivePackage;
            yield return null;
        }

        private IEnumerator _ApplicationStartupCo()
        {
            yield return _InitLogManagerCo();
            yield return _InitUIManagerCo();
            yield return _InitNetworkManagerCo();
            yield return _InitGMManagerCo();
            yield return _EnterGameCo();
        }
        
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            StartCoroutine(_ApplicationStartupCo());
        }

        void Update()
        {
            NetworkMgr.Tick();
            TimerMgr.Tick();
        }

        private void OnDestroy()
        {
            UIManager.Release();
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

