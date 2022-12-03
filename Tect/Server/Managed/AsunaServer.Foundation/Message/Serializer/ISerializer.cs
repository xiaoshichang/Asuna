
namespace AsunaServer.Foundation.Message.Serializer
{
    public interface ISerializer
    {
        public byte[] Serialize(object obj);
        public object Deserialize(byte[] data, int length, Type type);
    }


}

