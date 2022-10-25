namespace XServer.Foundation.Network
{
    public class TcpSession
    {
        public TcpSession(IntPtr session)
        {
            _Session = session;
        }

        public void OnReceive(byte[] data, int length)
        {
            
        }

        public void Send()
        {
            Send(null, 0);
        }
        
        private void Send(byte[] data, int length)
        {
        }

        private void OnDisconnect()
        {
            
        }

        public void Disconnect()
        {
            
        }
        
        private IntPtr _Session;
    }
}

