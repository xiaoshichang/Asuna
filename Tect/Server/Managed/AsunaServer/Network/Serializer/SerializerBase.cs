
using System.Reflection;

namespace AsunaServer.Network
{
    public abstract class SerializerBase
    {
        public abstract byte[] Serialize(object obj);
        public abstract object Deserialize(byte[] data, int length, uint typeIndex);
        public abstract void Collect(List<Assembly> assemblies, string ns);

        protected abstract void _Register(Type type);

        public uint GetIndexByType(Type type)
        {
            if (_Type2Index.TryGetValue(type, out var index))
            {
                return index;
            }

            throw new Exception($"index not found!{type.FullName}");
        }
        
        
        protected readonly Dictionary<Type, uint> _Type2Index = new();
        protected readonly Dictionary<uint, Type> _Index2Type = new();
    }


}

