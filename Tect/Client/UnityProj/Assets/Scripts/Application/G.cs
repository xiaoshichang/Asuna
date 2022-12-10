using AsunaClient.Application.Entity;
using AsunaClient.Application.GM;
using AsunaClient.Foundation.Asset;
using AsunaClient.Foundation.Network;
using AsunaClient.Foundation.UI;

namespace AsunaClient.Application
{
    public static class G
    {
        public static void Setup(ApplicationRoot root)
        {
            AssetManager = root.AssetManager;
            NetworkManager = root.NetworkManager;
            UIManager = root.UIManager;
            GMManager = root.GMManager;
            EntityManager = root.EntityManager;
        }
        
        public static AssetManager AssetManager;
        public static NetworkManager NetworkManager;
        public static UIManager UIManager;
        public static GMManager GMManager;
        public static EntityManager EntityManager;

    }
}