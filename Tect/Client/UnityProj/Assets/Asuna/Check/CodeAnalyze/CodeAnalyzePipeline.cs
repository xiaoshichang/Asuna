using System.Collections.Generic;
using System.IO;
using Asuna.Foundation.Debug;


namespace Asuna.Check
{
    public class CodeSpecificationCheckPipeline : CheckPipeline
    {
        public CodeSpecificationCheckPipeline()
        {
            AddTask("Code Specification Check", _CodeSpecificationCheck);
            AddTask("Generate Report", _GenerateReport);
        }

        private bool _CodeSpecificationCheck()
        {
            var checkToolPath = Path.Join(UnityEngine.Application.dataPath, "../../../Shared/Tools/CodeAnalyzer/CodeAnalyzerChecker/bin/Debug/net6.0/CodeAnalyzerChecker.exe");
            var solutionPath = Path.Join(UnityEngine.Application.dataPath, "../UnityProj.sln");
            var project = "Demo";
            var analyzerPath = Path.Join(UnityEngine.Application.dataPath, "../../../Shared/Tools/CodeAnalyzer/CodeAnalyzer/CodeAnalyzer/bin/Debug/netstandard2.0/CodeAnalyzer.dll");
            if (!File.Exists(solutionPath))
            {
                ADebug.Error($"{solutionPath} is not exist");
                return false;
            }
            if (!File.Exists(checkToolPath))
            {
                ADebug.Error($"{checkToolPath} is not exist");
                return false;
            }
            if (!File.Exists(analyzerPath))
            {
                ADebug.Error($"{analyzerPath} is not exist");
                return false;
            }
            
            //https://stackoverflow.com/questions/3173775/how-to-run-external-program-via-a-c-sharp-program
            using(System.Diagnostics.Process pProcess = new System.Diagnostics.Process())
            {
                pProcess.StartInfo.FileName = $"{checkToolPath}";
                pProcess.StartInfo.Arguments = $"{solutionPath} {project} {analyzerPath}";
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.RedirectStandardError = true;
                pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                pProcess.StartInfo.CreateNoWindow = true;
                pProcess.Start();
                pProcess.WaitForExit();
                if (pProcess.ExitCode == 0)
                {
                    while (!pProcess.StandardOutput.EndOfStream)
                    {
                        var message = pProcess.StandardOutput.ReadLine();
                        _Reports.Add(message);
                    }
                    return true;
                }
                else
                {
                    var err = pProcess.StandardError.ReadToEnd();
                    ADebug.Error($"from external check tool: {err}");
                    return false;
                }
            }
            
        }

        private bool _GenerateReport()
        {
            ADebug.Info("===================== Reports =========================");
            foreach (var report in _Reports)
            {
                ADebug.Warning(report);
            }
            ADebug.Info("=======================================================");
            return true;
        }


        private readonly List<string> _Reports = new List<string>();

    }
}