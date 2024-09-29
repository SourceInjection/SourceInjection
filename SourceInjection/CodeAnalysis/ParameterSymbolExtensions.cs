using Microsoft.CodeAnalysis;

namespace SourceInjection.CodeAnalysis
{
    internal static class ParameterSymbolExtensions
    {
        public static bool SupportsNullable(this IParameterSymbol parameter)
        {
            return !parameter.Type.HasDisallowNullAttribute()
                && (parameter.Type.HasNullableAnnotation() || parameter.Type.HasAllowNullAttribute());
        }
    }
}
