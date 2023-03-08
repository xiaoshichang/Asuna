using System;
using System.Collections.Generic;
using System.Reflection;
using Asuna.Foundation.Debug;
using Asuna.Utils;

namespace Asuna.Network
{
    public static class RpcTable
    {
        public static void Register(List<Assembly> assemblies)
        {
            _RegisterRPCMethods(assemblies);
            _RegisterBuiltinRPCType();
            _RegisterRPCType(assemblies);
            _PrintDebugInfo();
        }

        private static void _PrintDebugInfo()
        {
            ADebug.Info($"RPC Methods Count: {_Index2Method.Count}");
            ADebug.Info($"RPC Type Count: {_Index2Type.Count}");
        }
        
        private static void _RegisterRPCMethods(List<Assembly> assemblies)
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
                            var index = HashFunction.RpcToUint(type.Name, method.Name);
                            _Index2Method[index] = method;
                        }
                    }
                }
            }
        }

        private static void _RegisterBuiltinRPCType()
        {
            var builtinType = new List<Type>()
            {
                typeof(int), 
                typeof(float), 
                typeof(string),
                typeof(Guid),
            };
            foreach (var type in builtinType)
            {
                var index = HashFunction.StringToUint(type.Name);
                _Index2Type[index] = type;
            }
        }

        private static void _RegisterRPCType(List<Assembly> assemblies)
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

        public static Type GetTypeByIndex(uint index)
        {
            if (_Index2Type.TryGetValue(index, out var type))
            {
                return type;
            }
            throw new ArgumentException();
        }

        public static MethodInfo GetMethodByIndex(uint index)
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
}