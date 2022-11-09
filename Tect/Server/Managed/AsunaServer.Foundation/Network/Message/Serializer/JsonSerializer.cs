

using AsunaServer.Foundation.Network.Message.Indexer;

namespace AsunaServer.Foundation.Network.Message.Serializer
{
    public class JsonSerializer : ISerializer
    {
        public byte[] Serialize(object obj)
        {
            var str = System.Text.Json.JsonSerializer.Serialize(obj);
            var data = System.Text.Encoding.UTF8.GetBytes(str);
            return data;
        }

        public object Deserialize(byte[] data, uint typeIndex)
        {
            var str = System.Text.Encoding.UTF8.GetString(data);
            var type = AssemblyRegisterIndexer.Instance.GetType(typeIndex);
            var obj = System.Text.Json.JsonSerializer.Deserialize(str, type);
            if (obj == null)
            {
                throw new Exception($"Deserialize error! type:{type.FullName}, content:{str}");
            }

            return obj;
        }

    }
}

