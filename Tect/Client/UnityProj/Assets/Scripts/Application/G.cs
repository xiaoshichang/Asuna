using AsunaClient.Application.Entity;
using AsunaClient.Application.GM;
using AsunaClient.Foundation.Network;
using AsunaClient.Foundation.UI;
using UnityEditor.SceneManagement;

namespace AsunaClient.Application
{
    public static class G
    {
        public static void Setup(ApplicationRoot root)
        {
            EntityManager = root.EntityManager;
            NetworkManager = root.NetworkManager;
            GMManager = root.GMManager;
            UIManager = root.UIManager;
        }
        
        public static EntityManager EntityManager;
        public static NetworkManager NetworkManager;
        public static GMManager GMManager;
        public static UIManager UIManager;
    }
}