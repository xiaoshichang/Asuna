
namespace Asuna.Network
{
    public abstract class NetworkEvent
    {
        
    }

    public class NetworkEventOnConnect : NetworkEvent
    {
        public OnConnectResult Result { set; get; }
        public string Message { set; get; }
    }

    public class NetworkEventReceiveMessage : NetworkEvent
    {
        public object Message { set; get; }
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