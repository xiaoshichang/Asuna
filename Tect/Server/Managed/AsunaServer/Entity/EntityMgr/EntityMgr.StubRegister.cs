using System.Reflection;
using AsunaServer.Debug;

namespace AsunaServer.Entity;

public static partial class EntityMgr
{
    public static void DebugPrint()
    {
        foreach (var item in _RegisteredServerStubsTypes)
        {
            Logger.Debug($"register stub {item.Key}");
        }
    }
    
    
    public static void RegisterStubs(List<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(ServerStubEntity)))
                {
                    if (_RegisteredServerStubsTypes.ContainsKey(type.Name))
                    {
                        Logger.Error($"stub name conflict! name: {type.Name}, namespace: {type.Namespace}");
                        continue;
                    }
                    _RegisteredServerStubsTypes.Add(type.Name, type);
                }
            }
        }
    }

    public static Type? GetStubByName(string stubName)
    {
        if (_RegisteredServerStubsTypes.TryGetValue(stubName, out var type))
        {
            return type;
        }
        
        return null;
    }

    public static IReadOnlyDictionary<string, Type> GetRegisteredServerStubs()
    {
        return _RegisteredServerStubsTypes;
    }

    public static Dictionary<string, Type> _RegisteredServerStubsTypes = new();
}