using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityCodeAnalyzer.Rules
{
    public class MethodNamingRule
    {
        public static void Init(AnalysisContext context)
        {
            context.RegisterSymbolAction(Analyze, SymbolKind.Method);
        }

        private static void AnalyzePrivateMethod(SymbolAnalysisContext context, IMethodSymbol methodSymbol, string desc)
        {
            if (methodSymbol.Name.Length < 2)
            {
                Report(context, methodSymbol.Locations[0], desc);
                return;
            }
            if (methodSymbol.Name[0] != '_')
            {
                Report(context, methodSymbol.Locations[0], desc);
                return;
            }
            if (!char.IsUpper(methodSymbol.Name[1]))
            {
                Report(context, methodSymbol.Locations[0], desc);
                return;
            }
        }

        private static void AnalyzePublicMethod(SymbolAnalysisContext context, IMethodSymbol methodSymbol, string desc)
        {
            if (!char.IsUpper(methodSymbol.Name[0]))
            {
                Report(context, methodSymbol.Locations[0], desc);
                return;
            }
        }

        private static void AnalyzeProtectedMethod(SymbolAnalysisContext context, IMethodSymbol methodSymbol, string desc)
        {
            if (methodSymbol.Name.Length < 2)
            {
                Report(context, methodSymbol.Locations[0], desc);
                return;
            }
            if (methodSymbol.Name[0] != '_')
            {
                Report(context, methodSymbol.Locations[0], desc);
                return;
            }
            if (!char.IsUpper(methodSymbol.Name[1]))
            {
                Report(context, methodSymbol.Locations[0], desc);
                return;
            }
        }

        private static void Analyze(SymbolAnalysisContext context)
        {
            var methodSymbol = (IMethodSymbol)context.Symbol;
            if (methodSymbol.MethodKind == MethodKind.Constructor   || 
                methodSymbol.MethodKind == MethodKind.Destructor    ||
                methodSymbol.MethodKind == MethodKind.PropertyGet   ||
                methodSymbol.MethodKind == MethodKind.PropertySet )
            {
                return;
            }

            if (methodSymbol.DeclaredAccessibility == Accessibility.Public)
            {
                AnalyzePublicMethod(context, methodSymbol, PublicMethodFormatDesc);
            }
            else if (methodSymbol.DeclaredAccessibility == Accessibility.Protected)
            {
                AnalyzeProtectedMethod(context, methodSymbol, ProtectedMethodFormatDesc);
            }
            else if (methodSymbol.DeclaredAccessibility == Accessibility.Private)
            {

                AnalyzePrivateMethod(context, methodSymbol, PrivateMethodFormatDesc);
            }
        }

        private static void Report(SymbolAnalysisContext context, Location location, string desc)
        {
            var diagnostic = Diagnostic.Create(Descriptor, location, desc);
            context.ReportDiagnostic(diagnostic);
        }

        public const string ID = "MethodNamingRule";
        public static DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(ID, Title, Message, Category.ToString(), Severity, true);
        public static string Title => "Method Naming";
        public static string Message => "violating method naming rule. {0}";
        public static string PrivateMethodFormatDesc = "Private method should starts with under score character followed by upper case character";
        public static string ProtectedMethodFormatDesc = "Protected method should starts with under score character followed by upper case character";
        public static string PublicMethodFormatDesc = "Public method should starts with upper case character";

        public static DiagnosticsCategory Category => DiagnosticsCategory.Naming;
        public static DiagnosticSeverity Severity => DiagnosticSeverity.Error;
    }
}
