using System;

namespace AsunaClient.Foundation.Network.Message.Indexer
{
    public interface  IIndexer
    {
        public abstract uint GetIndexByType(Type type);
        public abstract Type GetTypeByIndex(uint index);
        
    }
}

