using System.Reflection;

namespace AsunaServer.Foundation.Message.Serializer
{
    public class JsonSerializer : ISerializer
    {
        public byte[] Serialize(object obj)
        {
            var str = System.Text.Json.JsonSerializer.Serialize(obj);
            var data = System.Text.Encoding.UTF8.GetBytes(str);
            return data;
        }

        public object Deserialize(byte[] data, int length, uint typeIndex)
        {
            var str = System.Text.Encoding.UTF8.GetString(data, 0, length);
            var type = _GetTypeByIndex(typeIndex);
            var obj = System.Text.Json.JsonSerializer.Deserialize(str, type);
            if (obj == null)
            {
                throw new Exception($"Deserialize error! type:{type.FullName}, content:{str}");
            }

            return obj;
        }
        
        public void Collect(List<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsSubclassOf(typeof(NetworkMessage)))
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

        public uint GetIndexByType(Type type)
        {
            if (_Type2Index.TryGetValue(type, out var index))
            {
                return index;
            }

            throw new Exception($"index not found!{type.FullName}");
        }

        private Type _GetTypeByIndex(uint index)
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
    }
}

