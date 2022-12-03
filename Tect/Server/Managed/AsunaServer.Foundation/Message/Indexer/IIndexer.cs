namespace AsunaServer.Foundation.Message.Indexer
{
    public interface  IIndexer
    {
        public abstract uint GetIndex(Type type);
        public abstract Type GetType(uint index);
        
    }
}

