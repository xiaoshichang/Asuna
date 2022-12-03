using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AsunaClient.Foundation;
using AsunaClient.Foundation.GM;
using AsunaClient.Foundation.Network;
using AsunaClient.Foundation.Network.Message;
using AsunaClient.Foundation.Network.Message.Indexer;
using AsunaClient.Foundation.UI;
using UnityEngine;


namespace AsunaClient.Application
{
    public class ApplicationRoot : MonoBehaviour
    {
        private IEnumerator _InitGMManagerCo()
        {
            var assemblyList = new List<string>()
            {
            };
            GMManager.Init(assemblyList);
            gameObject.AddComponent<GMTerminal>();
            XDebug.Info("Init GM Ok!");
            yield return null;
        }

        private IEnumerator _InitLogManagerCo()
        {
            XDebug.Init();
            yield return null;
        }

        private IEnumerator _InitUIManagerCo()
        {
            UIManager.Init();
            XDebug.Info("Init UI Ok!");
            yield return null;
        }
        
        private void _OnReceiveNetworkMessage(NetworkMessage message)
        {
        }
        
        private IEnumerator _InitNetworkCo()
        {
            List<Assembly> assemblies = new List<Assembly>();
            AssemblyRegisterIndexer.Instance.Collect(assemblies, typeof(NetworkMessage));
            
            NetworkMgr.Init(_OnReceiveNetworkMessage);
            XDebug.Info("Init Network Ok!");
            yield return null;
        }

        private IEnumerator _EnterGameCo()
        {
            yield return null;
            NetworkMgr.ConnectToAsync("127.0.0.1", 50001, OnConnected);
        }

        private void OnConnected(OnConnectResult cr)
        {
            XDebug.Info($"OnConnected {cr}");
        }

        private IEnumerator _ApplicationStartupCo()
        {
            yield return _InitLogManagerCo();
            yield return _InitNetworkCo();
            yield return _InitUIManagerCo();
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
            TimerMgr.Tick();
            NetworkMgr.Tick();
        }

        private void OnApplicationQuit()
        {
            NetworkMgr.Release();
            UIManager.Release();
        }


    }
}

