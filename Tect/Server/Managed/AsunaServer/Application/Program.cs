using System.Diagnostics;
using AsunaServer.Application;
using AsunaServer.Foundation.Debug;


namespace AsunaServer.Application // Note: actual namespace depends on the project name.
{

    public static class Program
    {
        
        private static ServerBase _CreateServerByServerConfig(ServerGroupConfig groupConfig, ServerConfigBase serverConfig)
        {
            G.GroupConfig = groupConfig;
            G.ServerConfig = serverConfig;
            ServerBase server;
            if (serverConfig is GMServerConfig)
            {
                server = new GMServer();
            }
            else if (serverConfig is GameServerConfig)
            {
                server = new GameServer();
            }
            else if (serverConfig is GateServerConfig)
            {
                server = new GateServer();
            }
            else
            {
                throw new Exception("unknown server type");
            }
            return server;
        }

        static (ServerGroupConfig, ServerConfigBase) _LoadConfig()
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

        private static void _LogPid()
        {
            ADebug.Info($"Pid: {Process.GetCurrentProcess().Id}");
        }

        private static void _LogPwd()
        {
            ADebug.Info($"CurrentDirectory: {Environment.CurrentDirectory}");
        }
        
        
        public static void Main(string[] args)
        {
            var (groupConfig, serverConfig) = _LoadConfig();
            
            var logTarget = groupConfig.Common.LogPath;
            var logFile = $"{groupConfig.Common.LogPath}/{serverConfig.Name}.log";
            ADebug.Init(logTarget, logFile);
            
            _LogPid();
            _LogPwd();
            
            var server = _CreateServerByServerConfig(groupConfig, serverConfig);
            server.Init();
            server.Run();
            server.Release();
        }
    }
    
    
}

