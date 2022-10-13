
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

#pragma warning disable CS8618

namespace Asuna.Foundation.Network.Rpc
{
    public class RpcRegisterEntry
    {
        public Type EntiryType;
        public MethodInfo MethodInfo;
        public RpcParam[]? RpcParams;
    }

    public struct RpcIndex
    {
        public RpcIndex(byte[] hashCode)
        {
            Index = BitConverter.ToInt32(hashCode, 0);
        }

        public int Index;
    }
    
    public static class RpcRegister
    {
        public static void CollectRpc()
        {
            var assemblyList = new List<string>()
            {
                "Asuna.GamePlay"
            };
            foreach (var item in assemblyList)
            {
                var assembly = Assembly.Load(item);
                foreach (var t in assembly.GetTypes())
                {
                    if (!t.IsSubclassOf(typeof(Entity)))
                    {
                        continue;
                    }
                    foreach (var method in t.GetMethods())
                    {
                        var rpcAttribute = method.GetCustomAttribute(typeof(RpcAttribute)) as RpcAttribute;
                        if ( rpcAttribute == null)
                        {
                            continue;
                        }

                        var rpcParams = _CollectRpcParams(method);
                        _RegisterRpc(t, method, rpcParams);
                    }
                }
            }
        }

        private static RpcParam[] _CollectRpcParams(MethodInfo methodInfo)
        {

        }

        private static RpcIndex _CalculateRpcIndex(Type entityType, MethodInfo methodInfo)
        {
            var name = entityType.Name + ":" + methodInfo.Name;
            var input = Encoding.UTF8.GetBytes(name);
            var hashed = _MD5Hasher.ComputeHash(input);
            var index = new RpcIndex(hashed);
            return index;
        }
        
        private static void _RegisterRpc(Type entityType, MethodInfo methodInfo, RpcParam[]? rpcParams) 
        {
            var index = _CalculateRpcIndex(entityType, methodInfo);
            var entry = new RpcRegisterEntry()
            {
                EntiryType = entityType,
                MethodInfo = methodInfo,
                RpcParams = rpcParams
            };
            
            if (_RpcIndexMap.ContainsKey(index))
            {
                throw new Exception("Hash Collision");
            }
            
            _RpcIndexMap.Add(index, entry);
        }

        private static readonly MD5 _MD5Hasher = MD5.Create();
        private static readonly Dictionary<RpcIndex, RpcRegisterEntry> _RpcIndexMap = new();
    }
}

