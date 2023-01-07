using System.Collections;
using UnityEngine;

namespace Asuna.Asset
{
    public abstract class AssetRequestHandler
    {
        protected AssetRequestHandler(AssetRequest request)
        {
            _Request = request;
        }
        
        public abstract IEnumerator AsyncOperation
        {
            get;
        }
        
        protected readonly AssetRequest _Request;

        public abstract void OnReleaseAsset();
    }
    
    public abstract class AssetRequestHandler<T> : AssetRequestHandler where T : Object
    {
        protected AssetRequestHandler(AssetRequest request) : base(request)
        {
        }
        
        public abstract T SyncLoad();
        
        public abstract T Asset
        {
            get;
            protected set;
        }
        
    }
}