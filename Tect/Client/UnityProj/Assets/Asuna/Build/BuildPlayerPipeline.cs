using System;
using System.IO;
using System.Linq;
using Asuna.Foundation.Debug;
using UnityEditor;
using UnityEditor.Build.Reporting;


namespace Asuna.Build
{
    public static class BuildPlayerPipeline
    {
        private static string GetCommandLineArgByKey(string key)
        {
            var args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == key)
                {
                    return args[i + 1];
                }
            }
            return string.Empty;
        }
        private static bool IsCommandLineArgExist(string key)
        {
            var args = Environment.GetCommandLineArgs();
            return args.Any(t => t == key);
        }
        
        private static BuildPlayerConfig _LoadBuildPlayerConfigFile()
        {
            ADebug.Info("_LoadBuildPlayerConfigFile start");
            var configPath = GetCommandLineArgByKey("-BuildConfig");
            ADebug.Info(configPath);
            var buildConfig = BuildPlayerConfig.Load(configPath);
            if (buildConfig == null)
            {
                ADebug.Error("_LoadBuildPlayerConfigFile buildConfig is null");
                EditorApplication.Exit(1);
                return null;
            }
            ADebug.Info($"_LoadBuildPlayerConfigFile end");
            return buildConfig;
        }

        private static void _SwitchTargetPlatform(BuildTarget target)
        {
            if (target == BuildTarget.StandaloneWindows64)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
            }
            else if (target == BuildTarget.Android)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            }
            else if (target == BuildTarget.iOS)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
            }
            else
            {
                ADebug.Error("_SwitchTargetPlatform, unknown build target");
                EditorApplication.Exit(1);
            }
            ADebug.Info($"_SwitchTargetPlatform switch to {EditorUserBuildSettings.activeBuildTarget} ok");
        }

        private static void _SetupScriptingDefineSymbols(BuildTarget target, string[] symbols)
        {
            if (target == BuildTarget.StandaloneWindows64)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbols);
            }
            else if (target == BuildTarget.Android)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbols);
            }
            else if (target == BuildTarget.iOS)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, symbols);
            }
            else
            {
                ADebug.Error("_SetupScriptingDefineSymbols, unknown build target");
                EditorApplication.Exit(1);
            }
        }
        
        /// <summary>
        /// BuildPlayerPipeline - refresh project settings
        /// </summary>
        public static void RefreshProjectSettingForBuild()
        {
            ADebug.Info("RefreshProjectSettingForBuild start");
            var config = _LoadBuildPlayerConfigFile();
            _SwitchTargetPlatform(config.BuildTarget);
            _SetupScriptingDefineSymbols(config.BuildTarget, config.ScriptingDefineSymbols);
            EditorApplication.Exit(0);
        }


        private static void _GenerateInternalBuildReport(BuildReport report)
        {
            
        }


        private static void _ClearDir(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
        }
        
        /// <summary>
        /// BuildPlayerPipeline - do build and report
        /// </summary>
        public static void BuildPlayer()
        {
            ADebug.Info("BuildPlayer start");
            var config = _LoadBuildPlayerConfigFile();
            _ClearDir(config.OutputDir);
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = new[] { "Assets/Asuna/Res/Scenes/Main.unity"};
            buildPlayerOptions.locationPathName = $"{config.OutputDir}/{PlayerSettings.productName}.exe";
            buildPlayerOptions.target = config.BuildTarget;
            buildPlayerOptions.targetGroup = BuildTargetGroup.Standalone;
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            _GenerateInternalBuildReport(report);
            ADebug.Info("BuildPlayer finish");
            EditorApplication.Exit(0);
        }
        
    }
}