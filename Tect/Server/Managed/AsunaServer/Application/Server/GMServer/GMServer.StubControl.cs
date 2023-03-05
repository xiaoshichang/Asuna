using AsunaServer.Message;
using AsunaServer.Entity;
using AsunaServer.Foundation.Debug;
using AsunaServer.Network;
using Google.Protobuf.Collections;

namespace AsunaServer.Application;

public partial class GMServer  : ServerBase
{
    
    private StubsDistributeNtf _GenStubsDistributeNotify()
    {
        var ntf = new StubsDistributeNtf();
        var allStubs = EntityMgr.GetRegisteredServerStubs();
        var allGames = G.GroupConfig.GameServers;
        var gameIndex = 0;
        
        foreach (var item in allStubs)
        {
            var stub = item.Key;
            var game = allGames[gameIndex].Name;
            ntf.StubsDistributeTable.Add(stub, game);

            gameIndex += 1;
            if (gameIndex >= allGames.Count)
            {
                gameIndex = 0;
            }
        }
        
        RpcCaller.StubsDistributeTable = ntf.StubsDistributeTable;
        return ntf;
    }

    private void _SendStubsDistributeNotifyToGames()
    {
        foreach (var game in Sessions.Games.Values)
        {
            var ntf = _GenStubsDistributeNotify();
            game.Send(ntf);
        }
    }

    private void _OnStubReady(TcpSession session, object message)
    {
        var ntf = message as StubReadyNtf;
        if (ntf == null)
        {
            throw new ArgumentException();
        }
        ADebug.Assert(!_ReadyStubs.Contains(ntf.StubName));
        _ReadyStubs.Add(ntf.StubName);
        if (_ReadyStubs.Count != RpcCaller.StubsDistributeTable.Count)
        {
            return;
        }
        ADebug.Info($"All Stubs is Ready! count: {_ReadyStubs.Count}");
        _OnAllStubReady();
    }

    private void _OnAllStubReady()
    {
        var ntf = new OpenGateNtf()
        {
            StubsDistributeTable = { RpcCaller.StubsDistributeTable }
        };
        foreach (var gate in Sessions.Gates.Values)
        {
            gate.Send(ntf);
        }
    }

    private readonly HashSet<string> _ReadyStubs = new();

}