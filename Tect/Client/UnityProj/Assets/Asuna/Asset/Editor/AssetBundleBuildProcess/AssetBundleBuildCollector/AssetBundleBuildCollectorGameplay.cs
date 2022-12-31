using System.Collections.Generic;
using UnityEditor;

namespace Asuna.Asset
{
    public class AssetBundleBuildCollectorGameplay: IAssetBundleBuildCollector
    {
        private const string DemoAssetBundleName = "Demo";
        
        private AssetBundleBuild _CollectGameplayAssetBundleBuild()
        {
            var dirs = new List<string>()
            {
                "Assets/Demo/Res"
            };
            
            var build = new AssetBundleBuild
            {
                assetBundleName = DemoAssetBundleName,
                assetNames = AssetBundleBuildHelper.CollectAllAssetFromDirs(dirs)
            };
            return build;
        }
        
        public List<AssetBundleBuild> Collect()
        {
            var builds = new List<AssetBundleBuild>();
            var build = _CollectGameplayAssetBundleBuild();
            builds.Add(build);
            return builds;
        }
    }
}