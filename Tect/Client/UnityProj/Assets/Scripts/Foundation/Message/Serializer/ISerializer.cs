
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AsunaClient.Foundation.Message.Serializer
{
    public abstract class SerializerBase
    {
        public abstract byte[] Serialize(object obj);
        public abstract object Deserialize(byte[] data, int length, uint typeIndex);
        public abstract void Collect(List<Assembly> assemblies);

        protected uint _ConvertTypeToHash(Type type)
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
        
        protected virtual void _Register(Type type)
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

        protected Type _GetTypeByIndex(uint index)
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
        
        protected readonly Dictionary<Type, uint> _Type2Index = new();
        protected readonly Dictionary<uint, Type> _Index2Type = new();
    }



}

