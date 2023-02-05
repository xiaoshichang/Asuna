using Asuna.Utils;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Asuna.Build
{
    public static class BuildPlayerPipeline
    {
        [MenuItem("Asuna/Build/BuildPlayer - Windows")]
        public static void BuildPlayerWindows()
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = new[] { "Assets/Asuna/Res/Scenes/Main.unity"};
            buildPlayerOptions.locationPathName = "windows64Build";
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.options = BuildOptions.None;
            BuildPlayer(buildPlayerOptions);
        }

        private static void BuildPlayer(BuildPlayerOptions buildPlayerOptions)
        {
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Succeeded)
            {
                ADebug.Info("Build Succeeded!");
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
    }
}