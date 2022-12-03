
using System;

namespace AsunaClient.Foundation.Network.Message.Serializer
{
    public class JsonSerializer : ISerializer
    {
        public byte[] Serialize(object obj)
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var data = System.Text.Encoding.UTF8.GetBytes(str);
            return data;
        }

        public object Deserialize(byte[] data, int length, Type type)
        {
            var str = System.Text.Encoding.UTF8.GetString(data, 0, length);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(str, type);
            if (obj == null)
            {
                throw new Exception($"Deserialize error! type:{type.FullName}, content:{str}");
            }

            return obj;
        }

        public static JsonSerializer Instance = new();
    }
}

