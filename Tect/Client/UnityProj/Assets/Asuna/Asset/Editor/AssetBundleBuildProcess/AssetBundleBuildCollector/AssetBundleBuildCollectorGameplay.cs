using System.Collections.Generic;
using UnityEditor;

namespace Asuna.Asset
{
    public class AssetBundleBuildCollectorGameplay: IAssetBundleBuildCollector
    {
        private AssetBundleBuild _CollectGameplayUIAssetBundleBuild()
        {
            var dirs = new List<string>()
            {
                "Assets/Demo/Res/UI"
            };
            
            var build = new AssetBundleBuild
            {
                assetBundleName = "Demo",
                assetNames = AssetBundleBuildHelper.CollectAllAssetFromDirs(dirs)
            };
            return build;
        }

        private AssetBundleBuild _CollectGameplaySceneAssetBundleBuild()
        {
            var dirs = new List<string>()
            {
                "Assets/Demo/Res/SceneData",
                "Assets/Demo/Res/SceneItems",
            };
            
            var build = new AssetBundleBuild
            {
                assetBundleName = "Demo.Scene",
                assetNames = AssetBundleBuildHelper.CollectAllAssetFromDirs(dirs)
            };
            return build;
        }
        
        private AssetBundleBuild _CollectGameplayCharacterAssetBundleBuild()
        {
            var dirs = new List<string>()
            {
                "Assets/Demo/Res/Character"
            };
            
            var build = new AssetBundleBuild
            {
                assetBundleName = "Demo.Character",
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
                _CollectGameplaySceneAssetBundleBuild(),
                _CollectGameplayUIAssetBundleBuild(),
                _CollectGameplayCharacterAssetBundleBuild()
            };
            return builds;
        }
    }
}