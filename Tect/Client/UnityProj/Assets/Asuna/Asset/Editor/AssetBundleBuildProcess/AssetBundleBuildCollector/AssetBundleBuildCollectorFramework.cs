using System.Collections.Generic;
using UnityEditor;

namespace Asuna.Asset
{
    /// <summary>
    /// 框架内部资源打包收集规则
    /// </summary>
    public class AssetBundleBuildCollectorFramework : IAssetBundleBuildCollector
    {
        private const string _FrameworkAssetBundleName = "Asuna";
        
        private AssetBundleBuild _CollectFrameworkAssetBundleBuild()
        {
            var dirs = new List<string>()
            {
                "Assets/Asuna/Res/UI"
            };
            
            var build = new AssetBundleBuild
            {
                assetBundleName = _FrameworkAssetBundleName,
                assetNames = AssetBundleBuildHelper.CollectAllAssetFromDirs(dirs)
            };
            return build;
        }
        
        public List<AssetBundleBuild> Collect()
        {
            var builds = new List<AssetBundleBuild>();
            var build = _CollectFrameworkAssetBundleBuild();
            builds.Add(build);
            return builds;
        }
    }
}