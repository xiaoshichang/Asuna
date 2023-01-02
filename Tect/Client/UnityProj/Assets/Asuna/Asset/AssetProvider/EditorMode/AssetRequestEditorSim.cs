using System.Collections;
using UnityEngine;

namespace Asuna.Asset
{
    public class AssetRequestEditorSim<T> : AssetRequest<T> where T : Object
    {
        public AssetRequestEditorSim(string assetPath, T asset) : base((assetPath))
        {
            Asset = asset;
        }
        
        public override YieldInstruction Operation => new WaitForFixedUpdate();
        public override T Asset { get; }
    }
}