using System.Reflection;

namespace AsunaServer.Foundation.Message.Serializer
{
    public class JsonSerializer : SerializerBase
    {
        public override byte[] Serialize(object obj)
        {
            var str = System.Text.Json.JsonSerializer.Serialize(obj);
            var data = System.Text.Encoding.UTF8.GetBytes(str);
            return data;
        }

        public override object Deserialize(byte[] data, int length, uint typeIndex)
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
        
        
    }
}

