using System;
using System.Collections.Generic;
using System.Reflection;

namespace Asuna.Foundation;


public class ServerStubDistributeTable
{
    
    /// <summary>
    /// collect all stubs and calculate distribute table
    /// </summary>
    public static ServerStubDistributeTable Collect(List<string> assemblyList, List<GameServerConfig> gameServers)
    {
        var table = new ServerStubDistributeTable();
        
        foreach (var item in assemblyList)
        {
            var assembly = Assembly.Load(item);
            foreach (var t in assembly.GetTypes())
            {
                if (t.IsSubclassOf(typeof(ServerStubEntity)))
                {
                    // todo: more smart Strategy
                    var index = Random.Shared.Next(gameServers.Count);
                    table.Items.Add(new KeyValuePair<Type, GameServerConfig>(t, gameServers[index]));
                }
            }
        }
        return table;
    }

    
    public List<KeyValuePair<Type, GameServerConfig>> Items = new();

}