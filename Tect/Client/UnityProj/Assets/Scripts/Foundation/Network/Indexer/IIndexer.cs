using System;

namespace AsunaClient.Foundation.Network
{
    public interface  IIndexer
    {
        public abstract uint GetIndexByType(Type type);
        public abstract Type GetTypeByIndex(uint index);
        
    }
}

