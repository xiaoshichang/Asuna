
using System;
using System.Collections.Generic;

namespace Asuna.Foundation.Network.Rpc
{
    
    public class RpcParam
    {
        public RpcParam(object obj)
        {
            InternalParam = obj;
            ParamType = obj.GetType();
        }
        
        public object InternalParam;
        public Type ParamType;
    }
}

