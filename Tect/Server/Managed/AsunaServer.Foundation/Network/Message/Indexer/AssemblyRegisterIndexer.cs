
using System.Reflection;
using System.Security.Cryptography;

namespace AsunaServer.Foundation.Network.Message.Indexer
{
    public class AssemblyRegisterIndexer : IIndexer
    {
        public void Init(List<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsSubclassOf(typeof(INetworkMessage)))
                    {
                        _Register(type);
                    }
                }
            }
        }

        private uint _ConvertTypeToHash(Type type)
        {
            var name = type.Name;
            uint hashedValue = 0;
            uint multiplier = 1;
            int i = 0;
            while (i < name.Length)
            {
                hashedValue += name[i] * multiplier;
                multiplier *= 37;
                i++;
            }
            return hashedValue;
        }
        
        private void _Register(Type type)
        {
            uint index = _ConvertTypeToHash(type);
            if (_Index2Type.ContainsKey(index))
            {
                throw new Exception($"duplicated index register. {type.Name}");
            }

            _Type2Index[type] = index;
            _Index2Type[index] = type;
        }

        public uint GetIndex(Type type)
        {
            if (_Type2Index.TryGetValue(type, out var index))
            {
                return index;
            }

            throw new Exception($"index not found!{type.FullName}");
        }

        public Type GetType(uint index)
        {
            if (_Index2Type.TryGetValue(index, out var type))
            {
                return type;
            }

            throw new Exception($"index not found!{index}");
        }

        public void DebugPrint()
        {
            Console.Out.WriteLine($"{_Index2Type.Count} types registered.");
            foreach (var kv in _Index2Type)
            {
                Console.Out.WriteLine($"{kv.Key} : {kv.Value.Name}");
            }
        }
        
        private readonly Dictionary<Type, uint> _Type2Index = new();
        private readonly Dictionary<uint, Type> _Index2Type = new();

        public static AssemblyRegisterIndexer Instance = new();
    }
}

