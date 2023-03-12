using AsunaServer.Utils;
using Google.Protobuf;

namespace AsunaServer.Network;

public static class RpcHelper
{
    public static void SerializeRpcArg(object arg, out byte[] data, out uint typeIndex)
    {
        typeIndex = HashFunction.StringToUint(arg.GetType().Name);
        var str = System.Text.Json.JsonSerializer.Serialize(arg);
        data = System.Text.Encoding.UTF8.GetBytes(str);
    }

    public static object? DeserializeRpcArg(ByteString data, uint typeIndex)
    {
        var type = RpcTable.GetTypeByIndex(typeIndex);
        var obj = System.Text.Json.JsonSerializer.Deserialize(data.ToStringUtf8(), type);
        return obj;
    }
}