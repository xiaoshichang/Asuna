using System.Reflection;
using AsunaServer.Foundation.Debug;

namespace AsunaServer.Entity;

public static partial class EntityMgr
{
    public static void DebugPrint()
    {
        foreach (var item in _RegisteredServerStubsTypes)
        {
            ADebug.Info($"register stub {item.Key}");
        }
    }
    
    
    public static void RegisterStubTypes(List<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(ServerStubEntity)))
                {
                    if (_RegisteredServerStubsTypes.ContainsKey(type.Name))
                    {
                        ADebug.Error($"stub name conflict! name: {type.Name}, namespace: {type.Namespace}");
                        continue;
                    }
                    _RegisteredServerStubsTypes.Add(type.Name, type);
                }
            }
        }
    }

    public static Type? GetStubTypeByName(string stubName)
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

    public static void RegisterStub(string name, ServerStubEntity stub)
    {
        _Stubs.Add(name, stub);
    }

    public static ServerStubEntity? GetStub(string name)
    {
        if (_Stubs.TryGetValue(name, out var stub))
        {
            return stub;
        }

        return null;
    }
    
    private static readonly Dictionary<string, ServerStubEntity> _Stubs = new();
    private static readonly Dictionary<string, Type> _RegisteredServerStubsTypes = new();
}