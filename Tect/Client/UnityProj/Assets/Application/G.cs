using AsunaClient.Application.Config;
using AsunaClient.Application.GM;
using AsunaClient.Foundation.Asset;
using AsunaClient.Foundation.Network;
using AsunaClient.Foundation.UI;
using AsunaClient.Foundation.Entity;


namespace AsunaClient.Application
{
    public static class G
    {
        
        /// <summary>
        /// core 代表可以在其他 manager 的初始化流程中引用
        /// </summary>
        public static void SetupCoreManagers(ApplicationRoot root)
        {
            ApplicationSetting = root.ApplicationSetting;
            AssetManager = root.AssetManager;
        }
        
        /// <summary>
        /// other 代表不能在其他 manager 的初始化流程中引用
        /// </summary>
        public static void SetupOtherManagers(ApplicationRoot root)
        {
            NetworkManager = root.NetworkManager;
            UIManager = root.UIManager;
            EntityManager = root.EntityManager;
        }

        /// <summary>
        /// system 是比 manager 更倾向与业务侧的模块
        /// </summary>
        public static void SetupSystems(ApplicationRoot root)
        {
            GMSystem = root.GMSystem;
        }

        public static void ResetCoreManagers()
        {
            ApplicationSetting = null;
            AssetManager = null;
        }

        public static void ResetOtherManagers()
        {
            NetworkManager = null;
            UIManager = null;
            EntityManager = null;
        }

        public static void ResetSystems()
        {
            GMSystem = null;
        }
        

        public static ApplicationSetting ApplicationSetting;
        public static AssetManager AssetManager;
        public static NetworkManager NetworkManager;
        public static UIManager UIManager;
        public static EntityManager EntityManager;
        
        public static GMSystem GMSystem;

    }
}