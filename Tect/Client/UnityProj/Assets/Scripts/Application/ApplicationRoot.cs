using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AsunaClient.Foundation;
using AsunaClient.Foundation.GM;
using AsunaClient.Foundation.Network;
using AsunaClient.Foundation.UI;
using AsunaShared.Message;
using Google.Protobuf.WellKnownTypes;
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
        
        private void _OnReceiveNetworkMessage(object message)
        {
        }
        
        private IEnumerator _InitNetworkCo()
        {
            List<Assembly> assemblies = new List<Assembly>()
            {
                Assembly.GetExecutingAssembly()
            };
            NetworkMgr.Serializer.Collect(assemblies);
            
            NetworkMgr.Init(_OnReceiveNetworkMessage);
            XDebug.Info("Init Network Ok!");
            yield return null;
        }

        private IEnumerator _EnterGameCo()
        {
            yield return null;
            NetworkMgr.ConnectToAsync("127.0.0.1", 50001, OnConnected);
        }

        private IEnumerator _PingPongTest()
        {
            while (true)
            {
                yield return new WaitForSeconds(3);
                var ping = new OuterPing()
                {
                    SendTime = Timestamp.FromDateTime(DateTime.UtcNow)
                };
                NetworkMgr.Send(ping);
            }
        }
        
        private void OnConnected(OnConnectResult cr)
        {
            XDebug.Info($"OnConnected {cr}");
            if (cr == OnConnectResult.OK)
            {
                StartCoroutine(_PingPongTest());
            }
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

