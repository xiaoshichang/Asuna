namespace AsunaServer.Foundation.Network.Message.Indexer
{
    public abstract class Indexer
    {
        public abstract uint GetIndex(Type type);
        public abstract Type GetType(uint index);
        
    }
}

