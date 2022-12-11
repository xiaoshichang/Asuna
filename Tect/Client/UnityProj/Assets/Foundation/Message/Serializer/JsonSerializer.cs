
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AsunaClient.Foundation.Message.Serializer
{
    public abstract class NetworkMessage
    {
    }
    
    public class JsonSerializer : SerializerBase
    {
        public override byte[] Serialize(object obj)
        {
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var data = System.Text.Encoding.UTF8.GetBytes(str);
            return data;
        }

        public override object Deserialize(byte[] data, int length, uint typeIndex)
        {
            var str = System.Text.Encoding.UTF8.GetString(data, 0, length);
            var type = _GetTypeByIndex(typeIndex);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(str, type);
            if (obj == null)
            {
                throw new Exception($"Deserialize error! type:{type.FullName}, content:{str}");
            }

            return obj;
        }

        public override void Collect(List<Assembly> assemblies)
        {
            var baseType = typeof(NetworkMessage);
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsSubclassOf(baseType))
                    {
                        _Register(type);
                    }
                }
            }
        }

    }
}

