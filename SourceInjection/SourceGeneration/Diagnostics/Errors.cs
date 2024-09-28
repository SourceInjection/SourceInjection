using Microsoft.CodeAnalysis;
using System.Linq;

namespace SourceInjection.SourceGeneration.Diagnostics
{
    internal static class Errors
    {
        public static Diagnostic MissingPartialModifier(ISymbol symbol, string generatorName)
        {
            return Diagnostic.Create(new DiagnosticDescriptor(
                    "ASG001",
                    Errors_en.Error_MissingPartialModifier_Title,
                    Errors_en.Error_MissingPartialModifier_Text,
                    Errors_en.Category_Codegeneration,
                    DiagnosticSeverity.Error,
                    true),
                symbol.Locations.FirstOrDefault(), generatorName, symbol.ToDisplayString());
        }

        public static Diagnostic NestedClassesAreNotSupported(ISymbol symbol, string generatorName)
        {
            return Diagnostic.Create(new DiagnosticDescriptor(
                "ASG002",
                Errors_en.Error_NestedTypesAreNotSupported_Title,
                Errors_en.Error_NestedTypesAreNotSupported_Text,
                Errors_en.Category_Codegeneration,
                DiagnosticSeverity.Error,
                true),
            symbol.Locations.FirstOrDefault(), generatorName, symbol.ToDisplayString());
        }
    }
}
