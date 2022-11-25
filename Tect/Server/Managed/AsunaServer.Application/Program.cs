using AsunaServer.Application.Config;
using AsunaServer.Foundation.Log;
using AsunaServer.Application.Server;


namespace AsunaServer.Application // Note: actual namespace depends on the project name.
{

    public static class Program
    {
        
        private static ServerBase CreateServerByServerConfig(ServerGroupConfig groupConfig, ServerConfigBase serverConfig)
        {
            ServerBase server;
            if (serverConfig is GMServerConfig gmServerConfig)
            {
                server = new GMServer(groupConfig, gmServerConfig);
            }
            else if (serverConfig is GameServerConfig gameServerConfig)
            {
                server = new GameServer(groupConfig, gameServerConfig);
            }
            else if (serverConfig is GateServerConfig gateServerConfig)
            {
                server = new GateServer(groupConfig, gateServerConfig);
            }
            else
            {
                throw new Exception("unknown server type");
            }
            return server;
        }

        static (ServerGroupConfig, ServerConfigBase) LoadConfig()
        {
            var configPath = Environment.GetEnvironmentVariable("ConfigPath");
            if (configPath == null)
            {
                throw new Exception("ConfigPath not found in EnvironmentVariable!");
            }
            var groupConfig = ServerGroupConfig.LoadConfig(configPath);
            if (groupConfig == null)
            {
                throw new Exception($"Load group config fail!");
            }
            var serverName = Environment.GetEnvironmentVariable("ServerName");
            if (serverName == null)
            {
                throw new Exception($"ServerName not found in EnvironmentVariable!");
            }
            var serverConfig = groupConfig.GetServerConfigByName(serverName);
            if (serverConfig == null)
            {
                throw new Exception($"{serverName} config not found in group config!");
            }
            return (groupConfig, serverConfig);
        }
        
        static void Main(string[] args)
        {
            var (groupConfig, serverConfig) = LoadConfig();
            
            var logTarget = groupConfig.Common.LogPath;
            var logFile = $"{groupConfig.Common.LogPath}/{serverConfig.Name}.log";
            Logger.Init(logTarget, logFile);
            
            var server = CreateServerByServerConfig(groupConfig, serverConfig);
            server.Init();
            server.Run();
            server.Release();
        }
    }
    
    
}

