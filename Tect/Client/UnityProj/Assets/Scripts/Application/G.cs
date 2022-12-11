using AsunaClient.Application.GM;
using AsunaClient.Foundation.Asset;
using AsunaClient.Foundation.Network;
using AsunaClient.Foundation.UI;
using AsunaClient.Foundation.Entity;


namespace AsunaClient.Application
{
    public static class G
    {
        public static void Setup(ApplicationRoot root)
        {
            AssetManager = root.AssetManager;
            NetworkManager = root.NetworkManager;
            UIManager = root.UIManager;
            EntityManager = root.EntityManager;
            
            GmSystem = root.GmSystem;
        }
        
        public static AssetManager AssetManager;
        public static NetworkManager NetworkManager;
        public static UIManager UIManager;
        public static EntityManager EntityManager;
        
        public static GMSystem GmSystem;

    }
}