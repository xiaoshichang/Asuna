using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityCodeAnalyzer.Rules
{
    public static class TypeNamingRule
    {
        public static void Init(AnalysisContext context)
        {
            context.RegisterSymbolAction(Analyze, SymbolKind.NamedType);
        }

        private static void Analyze(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            if (!char.IsUpper(namedTypeSymbol.Name[0]))
            {
                Report(context, namedTypeSymbol.Locations[0]);
                return;
            }
        }

        private static void Report(SymbolAnalysisContext context, Location location)
        {
            var diagnostic = Diagnostic.Create(Descriptor, location);
            context.ReportDiagnostic(diagnostic);
        }

        public const string ID = "TypeNamingRule";
        public static DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(ID, Title, Message, Category.ToString(), Severity, true);
        public static string Title => "Type Naming";
        public static string Message => "violating type naming rule. First character should be capital character.";
        public static DiagnosticsCategory Category => DiagnosticsCategory.Naming;
        public static DiagnosticSeverity Severity => DiagnosticSeverity.Error;
    }
}
