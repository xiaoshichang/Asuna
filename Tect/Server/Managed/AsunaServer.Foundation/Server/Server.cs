using XServer.Foundation.Network;

namespace AsunaServer.Foundation.Server
{
    public static class Server
    {
        public static void Init()
        {
            Core.Interface.Server_Init();
            Network.Init();
        }

        public static void Run()
        {
            Core.Interface.Server_Run();
        }

        public static void Release()
        {
            Core.Interface.Server_Finalize();
        }
        
    }
}

