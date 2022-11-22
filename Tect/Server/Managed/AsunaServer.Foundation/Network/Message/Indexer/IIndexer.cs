namespace AsunaServer.Foundation.Network.Message.Indexer
{
    public interface  IIndexer
    {
        public abstract uint GetIndex(Type type);
        public abstract Type GetType(uint index);
        
    }
}

