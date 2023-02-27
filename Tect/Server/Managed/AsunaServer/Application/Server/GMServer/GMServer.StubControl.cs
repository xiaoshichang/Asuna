using AsunaServer.Message;
using AsunaServer.Entity;
using AsunaServer.Debug;
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
        
        G.StubsDistributeTable = ntf.StubsDistributeTable;
        return ntf;
    }

    private void _SendStubsDistributeNotifyToGames()
    {
        foreach (var game in G.Games.Values)
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
        Logger.Assert(!_ReadyStubs.Contains(ntf.StubName));
        _ReadyStubs.Add(ntf.StubName);
        if (_ReadyStubs.Count != G.StubsDistributeTable.Count)
        {
            return;
        }
        Logger.Info($"All Stubs is Ready! count: {_ReadyStubs.Count}");
        _OnAllStubReady();
    }

    private void _OnAllStubReady()
    {
        var ntf = new OpenGateNtf()
        {
            StubsDistributeTable = { G.StubsDistributeTable }
        };
        foreach (var gate in G.Gates.Values)
        {
            gate.Send(ntf);
        }
    }

    private readonly HashSet<string> _ReadyStubs = new();

}