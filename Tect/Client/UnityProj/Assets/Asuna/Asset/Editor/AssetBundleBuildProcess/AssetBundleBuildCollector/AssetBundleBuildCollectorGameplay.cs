using System.Collections.Generic;
using UnityEditor;

namespace Asuna.Asset
{
    public class AssetBundleBuildCollectorGameplay: IAssetBundleBuildCollector
    {
        private AssetBundleBuild _CollectGameplayAssetBundleBuild()
        {
            var dirs = new List<string>()
            {
                "Assets/Demo/Res/SceneData",
                "Assets/Demo/Res/SceneItems",
                "Assets/Demo/Res/UI"
            };
            
            var build = new AssetBundleBuild
            {
                assetBundleName = "Demo",
                assetNames = AssetBundleBuildHelper.CollectAllAssetFromDirs(dirs)
            };
            return build;
        }

        private AssetBundleBuild _CollectGameplayCommonAssetBundleBuild()
        {
            var dirs = new List<string>()
            {
                "Assets/Demo/Res/Common"
            };
            
            var build = new AssetBundleBuild
            {
                assetBundleName = "Demo.Common",
                assetNames = AssetBundleBuildHelper.CollectAllAssetFromDirs(dirs)
            };
            return build;
        }
        
        public List<AssetBundleBuild> Collect()
        {
            var builds = new List<AssetBundleBuild>
            {
                _CollectGameplayCommonAssetBundleBuild(),
                _CollectGameplayAssetBundleBuild()
            };
            return builds;
        }
    }
}