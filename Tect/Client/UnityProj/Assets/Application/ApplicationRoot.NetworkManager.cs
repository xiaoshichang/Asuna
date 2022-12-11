using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AsunaClient.Foundation;
using AsunaClient.Foundation.Network;
using AsunaShared.Message;
using Google.Protobuf.WellKnownTypes;
using UnityEngine;

namespace AsunaClient.Application
{
    public partial class ApplicationRoot
    {
        private void _InitNetwork()
        {
            NetworkManager = new NetworkManager();
            List<Assembly> assemblies = new List<Assembly>()
            {
                Assembly.GetExecutingAssembly()
            };
            NetworkManager.Serializer.Collect(assemblies);
            var param = new NetworkManagerInitParam()
            {
                OnReceive = _OnReceiveNetworkMessage
            };
            NetworkManager.Init(param);
            XDebug.Info("Init Network Manager Ok!");
        }


        private void _ReleaseNetworkManager()
        {
            NetworkManager.Release();
            NetworkManager = null;
            XDebug.Info("Release Network Manager Ok!");
        }
        
        private void _OnReceiveNetworkMessage(object message)
        {
        }
        
        private void OnConnected(OnConnectResult cr)
        {
            XDebug.Info($"OnConnected {cr}");
            if (cr == OnConnectResult.OK)
            {
                StartCoroutine(_PingPongTest());
            }
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
                NetworkManager.Send(ping);
            }
        }

        public NetworkManager NetworkManager;
    }
}