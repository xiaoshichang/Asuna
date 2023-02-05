using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

internal class Program
{
    private static VisualStudioInstance? SelectVisualStudioInstance(VisualStudioInstance[] visualStudioInstances)
    {
        if (visualStudioInstances.Length >= 1)
        {
            return visualStudioInstances[0];
        }
        else
        {
            return null;
        }
    }

    static int Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.Error.WriteLine("args mismatch");
            return 1;
        }

        var solutionPath = args[0];
        if (!File.Exists(solutionPath))
        {
            Console.Error.WriteLine($"solutionPath does not exist!{solutionPath}");
            return 1;
        }
        var projectName = args[1];

        var analyzerPath = args[2];
        if (!File.Exists(analyzerPath))
        {
            Console.Error.WriteLine($"analyzerPath does not exist!{analyzerPath}");
            return 1;
        }

        // Attempt to set the version of MSBuild.
        var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
        var instance = SelectVisualStudioInstance(visualStudioInstances);
        if (instance == null)
        {
            Console.Error.WriteLine("VisualStudioInstance is null");
            return 1;
        }

        // NOTE: Be sure to register an instance with the MSBuildLocator 
        //       before calling MSBuildWorkspace.Create()
        //       otherwise, MSBuildWorkspace won't MEF compose.
        MSBuildLocator.RegisterInstance(instance);

        var workspace = MSBuildWorkspace.Create();
        var solution = workspace.OpenSolutionAsync(solutionPath).Result;
        Project? project = null;
        foreach (var p in solution.Projects)
        {
            if (p.Name == projectName)
            {
                project = p;
                break;
            }
        }
        if (project == null)
        {
            Console.Error.WriteLine("project not found");
            return 1;
        }

        var compilation = project.GetCompilationAsync().Result;
        if (compilation == null)
        {
            Console.Error.WriteLine("Compilation fail");
            return 1;
        }

        //https://stackoverflow.com/questions/62409948/how-to-add-custom-roslyn-analyzer-from-locally-placed-dll
        var assembly = Assembly.LoadFrom(analyzerPath);
        if (assembly == null)
        {
            Console.Error.WriteLine("Analyzer Assembly not found");
            return 1;
        }
        var analyzers = assembly.GetTypes()
                                .Where(t => t.GetCustomAttribute<DiagnosticAnalyzerAttribute>() is object)
                                .Select(t => (DiagnosticAnalyzer)Activator.CreateInstance(t))
                                .ToArray();

        if (analyzers.Length == 0)
        {
            Console.Error.WriteLine("Analyzer not found");
            return 1;
        }

        var compilationWithAnalyzers = compilation.WithAnalyzers(ImmutableArray.Create(analyzers));
        var reports = compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().Result;

        foreach (var report in reports)
        {
            var message = report.ToString();
            Console.Out.WriteLine(message);
        }
        return 0;
    }




}