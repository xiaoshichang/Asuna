// See https://aka.ms/new-console-template for more information

using AsunaServer.Foundation;


namespace AsunaServer.Application // Note: actual namespace depends on the project name.
{

    public static class Program
    {
        static void Main(string[] args)
        {
            Server.Init();
            TimerMgr.AddTimer(1000, TestTimeout, null);
            Server.Run();
            Server.Release();
        }

        static void TestTimeout(object? param)
        {
            Logger.Info("timeout");
        }
    }
    
    
}

