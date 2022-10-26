// See https://aka.ms/new-console-template for more information

using AsunaServer.Foundation.Log;
using AsunaServer.Foundation.Server;
using AsunaServer.Foundation.Timer;


namespace AsunaServer.Application // Note: actual namespace depends on the project name.
{

    public static class Program
    {
        static void Main(string[] args)
        {
            var server = new GameServer();
            TimerMgr.AddTimer(1000, TestTimeout, null);
            server.Run();
            server.Release();
        }

        static void TestTimeout(object? param)
        {
            Logger.Info("timeout");
        }
    }
    
    
}

