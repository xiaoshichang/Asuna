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
            server.Init();
            server.Run();
            server.Release();
        }
    }
    
    
}

