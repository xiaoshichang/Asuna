using AF.Application;
using AF.Application.GM;
using AF.Asset;
using AF.Network;
using AF.UI;
using AF.Entity;
using AF.Scene;


namespace AF.Application
{
    public static class G
    {

        public static void SetupConfig(ApplicationRoot root)
        {
            Application = root;
            ApplicationSetting = root.ApplicationSetting;
        }
        
        /// <summary>
        /// core 代表可以在其他 manager 的初始化流程中引用
        /// </summary>
        public static void SetupCoreManagers(ApplicationRoot root)
        {
            
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
        /// system 是 gameplay 层面的东西，更偏向于逻辑系统
        /// </summary>
        public static void SetupGameplay(GameplayInstance gameplayInstance)
        {
            GameplayInstance = gameplayInstance;
            GMSystem = gameplayInstance.GMSystem;
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

        public static void ResetGameplay()
        {
            GMSystem = null;
            
        }

        public static ApplicationSetting ApplicationSetting;
        public static ApplicationRoot Application;
        public static AssetManager AssetManager;
        public static NetworkManager NetworkManager;
        public static UIManager UIManager;
        public static EntityManager EntityManager;
        public static GameplayInstance GameplayInstance;
        
        public static GMSystem GMSystem;

    }
}