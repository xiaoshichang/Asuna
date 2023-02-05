using AsunaServer.Entity;
using AsunaServer.Network;
using AsunaServer.Timer;

namespace AsunaServer.Application.SystemStubs.HallStub;

public class HallStub : ServerStubEntity
{

    public override void Init()
    {
        _OnStubReady();
    }
}