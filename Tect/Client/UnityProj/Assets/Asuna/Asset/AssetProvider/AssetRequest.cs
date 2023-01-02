using System.Collections;
using UnityEngine;

namespace Asuna.Asset
{
    public abstract class AssetRequest<T> where T : Object
    {
        protected AssetRequest(string assetPath)
        {
            _AssetPath = assetPath;
        }
        
        public abstract YieldInstruction Operation
        {
            get;
        }

        public abstract T Asset
        {
            get;
        }

        private readonly string _AssetPath;

    }
}