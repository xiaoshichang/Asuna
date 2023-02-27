using System.Reflection;
using AsunaServer.Debug;
using AsunaServer.Utils;

namespace AsunaServer.Network;

public class RpcTable
{
    public void Register(List<Assembly> assemblies)
    {
        _RegisterRPCMethods(assemblies);
        _RegisterBuiltinRPCType();
        _RegisterRPCType(assemblies);
        _PrintDebugInfo();
    }

    private void _PrintDebugInfo()
    {
        Logger.Debug($"RPC Methods Count: {_Index2Method.Count}");
        Logger.Debug($"RPC Type Count: {_Index2Type.Count}");
    }
    
    private void _RegisterRPCMethods(List<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    var rpc = method.GetCustomAttribute<RpcAttribute>();
                    if (rpc != null)
                    {
                        var index = HashFunction.MethodToUint(method);
                        _Index2Method[index] = method;
                    }
                }
            }
        }
    }

    private void _RegisterBuiltinRPCType()
    {
        var builtinType = new List<Type>() { typeof(int), typeof(float), typeof(string) };
        foreach (var type in builtinType)
        {
            var index = HashFunction.StringToUint(type.Name);
            _Index2Type[index] = type;
        }
    }

    private void _RegisterRPCType(List<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                var rpc = type.GetCustomAttribute<RpcAttribute>();
                if (rpc != null)
                {
                    var index = HashFunction.StringToUint(type.Name);
                    _Index2Type[index] = type;
                }
            }
        }
    }

    public Type GetTypeByIndex(uint index)
    {
        if (_Index2Type.TryGetValue(index, out var type))
        {
            return type;
        }
        throw new ArgumentException();
    }

    public MethodInfo? GetMethodByIndex(uint index)
    {
        if (_Index2Method.TryGetValue(index, out var method))
        {
            return method;
        }

        return null;
    }

    private static readonly Dictionary<uint, Type> _Index2Type = new();
    private static readonly Dictionary<uint, MethodInfo> _Index2Method = new();

}