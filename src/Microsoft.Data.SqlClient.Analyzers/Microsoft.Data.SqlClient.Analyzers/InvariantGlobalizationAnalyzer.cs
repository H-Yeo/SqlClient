using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Microsoft.Data.SqlClient.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class InvariantGlobalizationAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "InvariantGlobalizationAnalyzer";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Microsoft.Data.SqlClient.Analyzers";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCompilationStartAction(CompilationStart);
        }

        private static void CompilationStart(CompilationStartAnalysisContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            bool isInvariantGlobalization = "Á".Equals("á", StringComparison.CurrentCultureIgnoreCase);
#if NET6_0_OR_GREATER
            // Microsoft Documentation on Breaking Changes for Invariant Globalization
            // https://learn.microsoft.com/en-us/dotnet/core/compatibility/globalization/6.0/culture-creation-invariant-mode
#else
            isInvariantGlobalization = true;
#endif

            bool isNativeAOT = isNativeAheadOfTimeCompilation();

            context.RegisterOperationAction(context =>
            {
                var invocation = (IInvocationOperation)context.Operation;

                if (isNativeAOT && isInvariantGlobalization)
                {
                    var diagnostic = Diagnostic.Create(Rule, invocation.Syntax.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }

            }, OperationKind.Invocation);
        }

        private static bool isNativeAheadOfTimeCompilation()
        {
            bool isNativeAOT = false;
#if NET6_0_OR_GREATER  
            var stackTrace = new StackTrace(false);
            isNativeAOT = stackTrace.GetFrame(0)?.GetMethod() is null;
#endif
            return isNativeAOT;
        }
    }
}
