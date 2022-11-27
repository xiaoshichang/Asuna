using System;
using System.Collections;
using System.Collections.Generic;
using AsunaClient.Foundation;
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
            yield return null;
        }

        private IEnumerator _EnterGameCo()
        {
            yield return null;
        }

        private IEnumerator _ApplicationStartupCo()
        {
            yield return _InitLogManagerCo();
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
        }

        private void OnDestroy()
        {
            UIManager.Release();
        }

        private void _ConnectCallback(Exception e)
        {
            if (e != null)
            {
                Debug.Log(e.Message);
                return;
            }
            Debug.Log("Connect to server ok!");
        }
    }
}

