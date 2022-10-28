namespace AsunaServer.Foundation.Network
{
    public class TcpSession
    {
        public TcpSession(IntPtr connection)
        {
            _Connection = connection;
        }

        public void OnAccept()
        {
            
        }

        public void OnDisconnect()
        {
            
        }
        
        private IntPtr _Connection;
    }
}

