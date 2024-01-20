using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AnalyzerCheckForTestName
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AnalyzerCheckForTestNameAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AnalyzerCheckForTestName";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        public static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterCompilationStartAction(startContext =>
            {
                startContext.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);

                startContext.RegisterCompilationEndAction(endContext =>
                {
                    var compilationWithAnalyzer = endContext.Compilation.WithAnalyzers(ImmutableArray.Create<DiagnosticAnalyzer>(this));
                    var diagnostics = compilationWithAnalyzer.GetAnalyzerDiagnosticsAsync().Result;
                    foreach (var diagnostic in diagnostics)
                    {
                        endContext.ReportDiagnostic(diagnostic);
                    }
                });
            });
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            if (namedTypeSymbol.Name == "TestName")
            {
                var diagnostic = Diagnostic.Create(Rule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
