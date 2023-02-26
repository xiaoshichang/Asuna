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
        
        _StubsDistributeTable = ntf.StubsDistributeTable;
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
        _ReadyStubs += 1;
        if (_StubsDistributeTable == null)
        {
            Logger.Warning("stub distribute table is not generated.");
            return;
        }
        if (_ReadyStubs != _StubsDistributeTable.Count)
        {
            return;
        }
        
        Logger.Info($"All Stubs is Ready! count: {_ReadyStubs}");
        _OnAllStubReady();

    }

    private void _OnAllStubReady()
    {
        var ntf = new OpenGateNtf();
        foreach (var gate in G.Gates.Values)
        {
            gate.Send(ntf);
        }
    }

    private MapField<string, string>? _StubsDistributeTable;
    private int _ReadyStubs = 0;

}