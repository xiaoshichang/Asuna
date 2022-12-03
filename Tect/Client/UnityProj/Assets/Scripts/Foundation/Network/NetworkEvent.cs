using AsunaClient.Foundation.Network.Message;

namespace AsunaClient.Foundation.Network
{
    public abstract class NetworkEvent
    {
        
    }

    public class NetworkEventReceiveMessage : NetworkEvent
    {
        public NetworkMessage Message { set; get; }
    }

    public class NetworkEventReceiveException : NetworkEvent
    {
        public string Message { set; get; }
    }

    public class NetworkEventSendException : NetworkEvent
    {
        public string Message { set; get; }
    }
    
    
}