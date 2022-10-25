namespace XServer.Foundation
{
    public static class Server
    {
        public static void Init()
        {
            Core.Interface.Server_Init();
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

