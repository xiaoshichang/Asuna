using AsunaServer.Foundation.Entity;
using AsunaServer.Foundation.Network;
using AsunaServer.Foundation.Timer;

namespace AsunaServer.Application.Server.SystemStubs.HallStub;

public class HallStub : ServerStubEntity
{

    public override void Init()
    {
        OnStubReady();
    }
}