using System;
using System.Collections;
using System.Collections.Generic;
using Asuna.Utils;
using UnityEditor;
using UnityEngine;

namespace Asuna.Asset
{
    public static class AssetBundleBuildProcess
    {
        private const string _MenuPath_Windows = "Asuna/Asset/Build AssetBundle/Windows";
        private const string _MenuPath_IOS = "Asuna/Asset/Build AssetBundle/IOS";
        private const string _MenuPath_Android = "Asuna/Asset/Build AssetBundle/Android";
        

        [MenuItem(_MenuPath_Windows)]
        private static void _BuildWindows()
        {
            var frameworkCollector = new AssetBundleBuildCollectorFramework();
            var frameworkBuilds = frameworkCollector.Collect();
            var gameplayCollector = new AssetBundleBuildCollectorGameplay();
            var gameplayBuilds = gameplayCollector.Collect();
            frameworkBuilds.AddRange(gameplayBuilds);
            var allBuilds = frameworkBuilds.ToArray();
            
            var options = _DecideBuildOptions();
            var output = AssetProviderAssetBundle.AssetBundleRootWindows;
            FileUtils.MakeSureDirectory(output);
            var manifest = BuildPipeline.BuildAssetBundles(output, allBuilds, options, BuildTarget.StandaloneWindows64);
            var bundles = manifest.GetAllAssetBundles();
            ADebug.Info($"Build AssetBundle OK! output path: {output}, bundles count: {bundles.Length}!");
        }

        [MenuItem(_MenuPath_IOS)]
        private static void _BuildIOS()
        {
            throw new NotImplementedException();
        }

        [MenuItem(_MenuPath_Android)]
        private static void _BuildAndroid()
        {
            throw new NotImplementedException();
        }

        private static BuildAssetBundleOptions _DecideBuildOptions()
        {
            var options = BuildAssetBundleOptions.None;
            return options;
        }
        
    }

}

