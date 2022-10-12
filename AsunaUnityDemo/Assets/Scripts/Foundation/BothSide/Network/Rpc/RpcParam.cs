
using System.Collections.Generic;

namespace Asuna.Foundation
{
    public enum RpcParamType
    {
        Int,
        String,
        Object
    }
    
    public class RpcParam
    {
        public RpcParam(int obj)
        {
            ParamType = RpcParamType.Int;
            InternalParam = obj;
        }

        public RpcParam(string obj)
        {
            ParamType = RpcParamType.String;
            InternalParam = obj;
        }

        public RpcParam(object obj)
        {
            ParamType = RpcParamType.Object;
            InternalParam = obj;
        }
        
        public RpcParamType ParamType;
        public object InternalParam;
    }
}

