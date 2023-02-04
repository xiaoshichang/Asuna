using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using UnityCodeAnalyzer.Rules;

namespace UnityCodeAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UnityCodeAnalyzer : DiagnosticAnalyzer
    {
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            InitRules(context);
        }

        private void InitRules(AnalysisContext context)
        { 
            // naming
            FieldNamingRule.Init(context);
            MethodNamingRule.Init(context);
            PropertyNamingRule.Init(context);
            TypeNamingRule.Init(context);
            // maintainability
            MethodComplexityRule.Init(context);
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics 
        { 
            get 
            {
                var descriptors = new List<DiagnosticDescriptor>()
                {
                    // naming
                    FieldNamingRule.Descriptor,
                    MethodNamingRule.Descriptor,
                    PropertyNamingRule.Descriptor,
                    TypeNamingRule.Descriptor,
                    // maintainability
                    MethodComplexityRule.Descriptor,
                };
                return ImmutableArray.Create<DiagnosticDescriptor>(descriptors.ToArray());
            } 
        }

    }
}
