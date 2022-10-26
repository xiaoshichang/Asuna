using AsunaServer.Foundation.Network;
using AsunaServer.Foundation.Timer;

namespace AsunaServer.Foundation.Server
{
    public abstract class ServerBase
    {
        public virtual void Init()
        {
            InnerNetwork.Init();
        }

        public void Run()
        {
            Core.Interface.Server_Run();
        }

        public virtual void Release()
        {
        }
        
    }
}

