using AsunaServer.Foundation.Debug;
using AsunaServer.Utils;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace AsunaServer.Network;

public static class RpcHelper
{
    public static void SerializeRpcArg(object arg, out byte[] data, out uint typeIndex)
    {
        typeIndex = HashUtils.StringToUint(arg.GetType().Name);
        var str = System.Text.Json.JsonSerializer.Serialize(arg);
        data = System.Text.Encoding.UTF8.GetBytes(str);
    }

    public static object? DeserializeRpcArg(ByteString data, uint typeIndex)
    {
        var type = RpcTable.GetTypeByIndex(typeIndex);
        var obj = System.Text.Json.JsonSerializer.Deserialize(data.ToStringUtf8(), type);
        return obj;
    }

    public static bool ConvertRpcArgs(int argCount,  RepeatedField<ByteString> args,  RepeatedField<uint> typeIndexes, out object[] outArgs)
    {
        outArgs = new object[argCount];
        for (var i = 0; i < argCount; i++)
        {
            var str = args[i].ToStringUtf8();
            var type = RpcTable.GetTypeByIndex(typeIndexes[i]);
            var obj = System.Text.Json.JsonSerializer.Deserialize(str, type);
            
            if (obj == null)
            {
                ADebug.Error($"_ConvertArgs {i}-th arg is null, do not supported.");
                return false;
            }
            else
            {
                outArgs[i] = obj;
            }
        }
        return true;
    }
}