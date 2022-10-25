
using System;

namespace Asuna.Foundation.Network.Rpc
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RpcAttribute : Attribute
    {
        public RpcAttribute()
        {
            
        }
    }
}

