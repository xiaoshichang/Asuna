using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityCodeAnalyzer.Rules
{
    public static class FieldNamingRule
    {
        public static void Init(AnalysisContext context)
        {
            context.RegisterSymbolAction(Analyze, SymbolKind.Field);
        }

        private static void AnalyzePrivateField(SymbolAnalysisContext context, IFieldSymbol fieldSymbol, string desc)
        {
            if (fieldSymbol.Name.Length < 2)
            {
                Report(context, fieldSymbol.Locations[0], desc);
                return;
            }
            if (fieldSymbol.Name[0] != '_')
            {
                Report(context, fieldSymbol.Locations[0], desc);
                return;
            }
            if (!char.IsUpper(fieldSymbol.Name[1]))
            {
                Report(context, fieldSymbol.Locations[0], desc);
                return;
            }
        }

        private static void AnalyzeProtectedField(SymbolAnalysisContext context, IFieldSymbol fieldSymbol, string desc)
        {
            if (fieldSymbol.Name.Length < 2)
            {
                Report(context, fieldSymbol.Locations[0], desc);
                return;
            }
            if (fieldSymbol.Name[0] != '_')
            {
                Report(context, fieldSymbol.Locations[0], desc);
                return;
            }
            if (!char.IsUpper(fieldSymbol.Name[1]))
            {
                Report(context, fieldSymbol.Locations[0], desc);
                return;
            }
        }

        private static void AnalyzePublicField(SymbolAnalysisContext context, IFieldSymbol fieldSymbol, string desc)
        {
            if (!char.IsUpper(fieldSymbol.Name[0]))
            {
                Report(context, fieldSymbol.Locations[0], desc);
                return;
            }
        }

        private static void Analyze(SymbolAnalysisContext context)
        {
            var fieldSymbol = (IFieldSymbol)context.Symbol;
            if (fieldSymbol.DeclaredAccessibility == Accessibility.Public)
            {
                AnalyzePublicField(context, fieldSymbol, PublicDesc);
            }
            else if (fieldSymbol.DeclaredAccessibility == Accessibility.Protected)
            {
                AnalyzeProtectedField(context, fieldSymbol, ProtectedDesc);
            }
            else if (fieldSymbol.DeclaredAccessibility == Accessibility.Private)
            {
                AnalyzePrivateField(context, fieldSymbol, PrivateDesc);
            }

        }

        private static void Report(SymbolAnalysisContext context, Location location, string desc)
        {
            var diagnostic = Diagnostic.Create(Descriptor, location, desc);
            context.ReportDiagnostic(diagnostic);
        }

        public const string ID = "FieldNamingRule";
        public static DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(ID, Title, Message, Category.ToString(), Severity, true);
        public static string Title => "Field Naming";
        public static string Message => "violating field naming rule. {0}";
        public static string PrivateDesc = "Private field should starts with under score character followed by upper case character";
        public static string ProtectedDesc = "Protected field should starts with under score character followed by upper case character";
        public static string PublicDesc = "Public field should starts with upper case character";
        public static DiagnosticsCategory Category => DiagnosticsCategory.Naming;
        public static DiagnosticSeverity Severity => DiagnosticSeverity.Error;
    }
}
