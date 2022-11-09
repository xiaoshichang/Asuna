
namespace AsunaServer.Foundation.Network.Message.Serializer
{
    public interface ISerializer
    {
        public byte[] Serialize(object obj);
        public object Deserialize(byte[] data, uint type);
    }


}

