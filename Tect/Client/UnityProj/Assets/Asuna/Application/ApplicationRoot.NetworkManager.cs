using System.Collections.Generic;
using System.Reflection;
using Asuna.Auth;
using Asuna.Foundation.Debug;
using Asuna.Network;
using AsunaShared.Message;
using Newtonsoft.Json;

namespace Asuna.Application
{
    public partial class ApplicationRoot
    {
        private void _RegisterNetworkMessageType()
        {
            // Message 目前都定义在框架层面
            List<Assembly> assemblies = new List<Assembly>()
            {
                Assembly.GetExecutingAssembly()
            };
            NetworkManager.MessageSerializer.Collect(assemblies);
        }
        
        private void _RegisterRpcType()
        {
            // Rpc 可能定义在框架和Gameplay
            List<Assembly> assemblies = new List<Assembly>()
            {
                Assembly.GetExecutingAssembly(),
            };
            foreach (var assemblyName in ApplicationSetting.GameplayAssemblies)
            {
                assemblies.Add(Assembly.Load(assemblyName));
            }
            RpcTable.Register(assemblies);
        }
        
        private void _RegisterPersistentMessageHandlers()
        {
            NetworkManager.RegisterMessageHandler(typeof(AccountRpc), Account._OnAccountRpc);
        }
        
        private void _InitNetwork()
        {
            NetworkManager = new NetworkManager();

            _RegisterNetworkMessageType();
            _RegisterRpcType();
            _RegisterPersistentMessageHandlers();

            NetworkManager.Init(null);
            ADebug.Info("Init Network Manager Ok!");
        }

        private void _ReleaseNetworkManager()
        {
            NetworkManager.Release();
            NetworkManager = null;
            ADebug.Info("Release Network Manager Ok!");
        }

        public NetworkManager NetworkManager;
    }
}