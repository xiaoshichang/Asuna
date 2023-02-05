using System.IO;
using Asuna.Utils;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Asuna.Build
{
    public static class BuildPlayerPipeline
    {
        [MenuItem("Asuna/Build/BuildPlayer - Windows")]
        public static void BuildPlayerWindows()
        {
            _ClearBuildDir();
            
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = new[] { "Assets/Asuna/Res/Scenes/Main.unity"};
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.locationPathName = $"Build/{buildPlayerOptions.target.ToString()}/{PlayerSettings.productName}.exe";
            buildPlayerOptions.options = BuildOptions.None;
            
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.IL2CPP);
            
            _BuildPlayer(buildPlayerOptions);
        }


        private static void _ClearBuildDir()
        {
            if (Directory.Exists("Build"))
            {
                Directory.Delete("Build");
            }
        }

        private static void _ReportBuildStepsDetail(BuildReport report)
        {
            ADebug.Info($"total steps: {report.steps.Length}");
            for (int i = 0; i < report.steps.Length; i++)
            {
                var step = report.steps[i];
                var prefix = "";
                for (int j = 0; j < step.depth; j++)
                {
                    prefix += "\t";
                }
                ADebug.Info($"{prefix}step[{i + 1}/{report.steps.Length}] - name: {step.name} | duration: {step.duration.TotalSeconds}");
            }
        }
        
        private static void _ReportBuildResult(BuildReport report)
        {
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Succeeded)
            {
                ADebug.Info("================================================");
                ADebug.Info("Build Succeeded!");
                ADebug.Info($"totalSize: {report.summary.totalSize}");
                ADebug.Info($"totalTime: {report.summary.totalTime}");
                ADebug.Info($"total warning count: {report.summary.totalWarnings}");
                ADebug.Info($"total error count: {report.summary.totalErrors}");
                _ReportBuildStepsDetail(report);
                ADebug.Info("================================================");
            }
            else if (summary.result == BuildResult.Failed)
            {
                ADebug.Error("Build Failed");
            }
            else if (summary.result == BuildResult.Cancelled)
            {
                ADebug.Warning("Build Cancelled");
            }
            else
            {
                ADebug.Error("Build Result Unknown");
            }
        }

        private static void _BuildPlayer(BuildPlayerOptions buildPlayerOptions)
        {
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            _ReportBuildResult(report);
        }
    }
}