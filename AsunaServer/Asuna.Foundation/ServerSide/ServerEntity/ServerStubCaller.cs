using System;
using System.Collections.Generic;

namespace Asuna.Foundation;

public static class ServerStubCaller
{
    public static void Register(List<KeyValuePair<string,string>> stubs)
    {
        foreach (var pair in stubs)
        {
            if (_Stub2ServerMap.ContainsKey(pair.Key))
            {
                ALogger.LogError("duplicated stub name");
                return;
            }
            _Stub2ServerMap.Add(pair.Key, pair.Value);
        }
    }

    private static void CallStub(string stub, string server, string rpc)
    {
        
    }

    public static void CallStub(string stub, string rpc)
    {
        if (_Stub2ServerMap.TryGetValue(stub, out var server))
        {
            if (string.IsNullOrEmpty(server))
            {
                throw new Exception("server invalid");
            }
            CallStub(stub, server, rpc);
        }
        else
        {
            throw new Exception("Stub not exist");
        }
    }

    private static readonly Dictionary<string, string> _Stub2ServerMap = new();
}