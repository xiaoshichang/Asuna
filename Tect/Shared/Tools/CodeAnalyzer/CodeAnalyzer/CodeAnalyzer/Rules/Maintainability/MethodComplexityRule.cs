using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityCodeAnalyzer.Rules
{
    public static class MethodComplexityRule
    {
        public static void Init(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.MethodDeclaration);
        }

        private static void Analyze(SyntaxNodeAnalysisContext context)
        {
            var methodDeclaration = (MethodDeclarationSyntax)context.Node;
            var localtion = methodDeclaration.GetLocation();
            var methodLength = localtion.GetLineSpan().Span.End.Line - localtion.GetLineSpan().Span.Start.Line;
            if (methodLength > ExceptedMethodLine)
            {
                var methodName = methodDeclaration.Identifier.ToString();
                var idLocation = methodDeclaration.Identifier.GetLocation();
                var diagnostic = Diagnostic.Create(Descriptor, idLocation, methodName, methodLength, ExceptedMethodLine);
                context.ReportDiagnostic(diagnostic);
            }
        }

        public const string ID = "MethodComplexityRule";
        public static DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(ID, Title, Message, Category.ToString(), Severity, true);
        public static string Title => "Method is too complex.";
        public static string Message => "Method ({0}) is too complex, containing {1} lines, should less then {2} lines";
        public static DiagnosticsCategory Category => DiagnosticsCategory.Maintainability;
        public static DiagnosticSeverity Severity => DiagnosticSeverity.Error;

        /// <summary>
        /// keep all the code of a method in screen.
        /// </summary>
        public const int ExceptedMethodLine = 120;

    }
}
