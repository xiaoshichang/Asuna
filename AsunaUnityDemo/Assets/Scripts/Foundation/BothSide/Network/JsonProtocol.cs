using System;
using System.Text;
using Newtonsoft.Json;

#pragma warning disable CS8618
#nullable enable

namespace Asuna.Foundation.Network
{
    /// <summary>
    /// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// |  Json Object Type(4Byte) |    json Object Data         |
    /// ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// </summary>
    public class PackageJson : PackageBase
    {
        public PackageJson()
        {
            PackageType = PackageType.Json;
        }

        public int JsonObjectType;
        public object JsonObject;
        
        public override byte[] DumpPayload()
        {
            if (JsonObject == null)
            {
                throw new Exception("empty Json Object");
            }

            var JsonObjectBuffer = Encoding.Default.GetBytes(JsonConvert.SerializeObject(JsonObject));
            var JsonObjectTypeBuffer = BitConverter.GetBytes(JsonObjectType);
            var payloadBuffer = new byte[JsonObjectTypeBuffer.Length + JsonObjectBuffer.Length];
            JsonObjectTypeBuffer.CopyTo(payloadBuffer, 0);
            JsonObjectBuffer.CopyTo(payloadBuffer, 4);
            return payloadBuffer;
        }

        public override int GetPayloadMsgType()
        {
            return BitConverter.ToInt32(Payload, 0);
        }

        public override T ParsePayload<T>()
        {
            var str = Encoding.Default.GetString(Payload, 4, Payload.Length - 4);
            var obj = JsonConvert.DeserializeObject<T>(str);
            if (obj == null)
            {
                throw new Exception("Parse payload fail");
            }
            return obj;
        }
    }

}