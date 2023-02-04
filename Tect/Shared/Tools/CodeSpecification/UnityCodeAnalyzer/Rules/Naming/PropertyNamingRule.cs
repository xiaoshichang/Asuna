using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnityCodeAnalyzer.Rules
{
    public class PropertyNamingRule
    {
        public static void Init(AnalysisContext context)
        {
            context.RegisterSymbolAction(Analyze, SymbolKind.Property);
        }

        private static void AnalyzePrivateProperty(SymbolAnalysisContext context, IPropertySymbol propertySymbol, string desc)
        {
            if (propertySymbol.Name.Length < 2)
            {
                Report(context, propertySymbol.Locations[0], desc);
                return;
            }
            if (propertySymbol.Name[0] != '_')
            {
                Report(context, propertySymbol.Locations[0], desc);
                return;
            }
            if (!char.IsUpper(propertySymbol.Name[1]))
            {
                Report(context, propertySymbol.Locations[0], desc);
                return;
            }
        }

        private static void AnalyzeProtectedProperty(SymbolAnalysisContext context, IPropertySymbol propertySymbol, string desc)
        {
            if (propertySymbol.Name.Length < 2)
            {
                Report(context, propertySymbol.Locations[0], desc);
                return;
            }
            if (propertySymbol.Name[0] != '_')
            {
                Report(context, propertySymbol.Locations[0], desc);
                return;
            }
            if (!char.IsUpper(propertySymbol.Name[1]))
            {
                Report(context, propertySymbol.Locations[0], desc);
                return;
            }
        }

        private static void AnalyzePublicProperty(SymbolAnalysisContext context, IPropertySymbol propertySymbol, string desc)
        {
            if (!char.IsUpper(propertySymbol.Name[0]))
            {
                Report(context, propertySymbol.Locations[0], desc);
                return;
            }
        }

        private static void Analyze(SymbolAnalysisContext context)
        {
            var propertySymbol = (IPropertySymbol)context.Symbol;
            if (propertySymbol.DeclaredAccessibility == Accessibility.Public)
            {
                AnalyzePublicProperty(context, propertySymbol, PublicDesc);
            }
            else if (propertySymbol.DeclaredAccessibility == Accessibility.Protected)
            {
                AnalyzeProtectedProperty(context, propertySymbol, ProtectedDesc);
            }
            else if (propertySymbol.DeclaredAccessibility == Accessibility.Private)
            {
                AnalyzePrivateProperty(context, propertySymbol, PrivateDesc);
            }

        }

        private static void Report(SymbolAnalysisContext context, Location location, string desc)
        {
            var diagnostic = Diagnostic.Create(Descriptor, location, desc);
            context.ReportDiagnostic(diagnostic);
        }

        public const string ID = "PropertyNamingRule";
        public static DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(ID, Title, Message, Category.ToString(), Severity, true);
        public static string Title => "Property Naming";
        public static string Message => "violating property naming rule. {0}";
        public static string PrivateDesc = "Private property should starts with under score character followed by upper case character";
        public static string ProtectedDesc = "Protected property should starts with under score character followed by upper case character";
        public static string PublicDesc = "Public property should starts with upper case character";
        public static DiagnosticsCategory Category => DiagnosticsCategory.Naming;
        public static DiagnosticSeverity Severity => DiagnosticSeverity.Error;
    }
}
