﻿
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

#pragma warning disable CS8618

namespace AsunaServer.Foundation.Config;

public class CommonConfig
{
    public string LogPath { get; set; } = string.Empty;
}

public class ServerConfigBase
{
    public string Name { get; set; } = string.Empty;
    public string InternalIP { get; set; } = string.Empty;
    public int InternalPort { get; set; }
}

public class GMServerConfig : ServerConfigBase
{
}

public class GameServerConfig : ServerConfigBase
{
}

public class GateServerConfig : ServerConfigBase
{
}

public class ServerGroupConfig
{
    public CommonConfig Common { get; set; }
    public GMServerConfig GMServer { get; set; }
    public List<GameServerConfig> GameServers { get; set; }
    public List<GateServerConfig> GateServers { get; set; }

    
    public ServerConfigBase? GetCurrentServerConfigByName(string servername)
    {
        if (GMServer.Name == servername)
        {
            return GMServer;
        }
        foreach (var config in GameServers)
        {
            if (config.Name == servername)
            {
                return config;
            }
        }
        foreach (var config in GateServers)
        {
            if (config.Name == servername)
            {
                return config;
            }
        }
        return null;
    }
    
    public static ServerGroupConfig? LoadConfig(string configPath)
    {
        string content = File.ReadAllText(configPath);
        var groupConfig = JsonSerializer.Deserialize<ServerGroupConfig>(content);
        return groupConfig;
    }
    
    public GMServerConfig GetGMConfig()
    {
        return GMServer;
    }

    public int GetServerGroupNodesCount()
    {
        return 1 + GameServers.Count + GateServers.Count;
    }
}
