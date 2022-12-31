using System.Collections.Generic;
using UnityEditor;

namespace Asuna.Asset
{
    public interface IAssetBundleBuildCollector
    {
        List<AssetBundleBuild> Collect();
    }

    

    
}