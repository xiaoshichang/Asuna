using AsunaServer.Foundation.Debug;
using Google.Protobuf;

namespace AsunaServer.Network
{
    public static class GuidConverter
    {
        public static ByteString ToProto(this Guid guid)
        {
            ADebug.Assert(guid.ToByteArray().Length == 16);
            return ByteString.CopyFrom(guid.ToByteArray());
        }

        public static Guid ToGuid(this ByteString data)
        {
            ADebug.Assert(data.ToByteArray().Length == 16);
            return new Guid(data.ToByteArray());
        }
    }
}
