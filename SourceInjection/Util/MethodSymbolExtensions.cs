using Microsoft.CodeAnalysis;
using SourceInjection.CodeAnalysis;
using System.Linq;

namespace SourceInjection.Util
{
    internal static class MethodSymbolExtensions
    {
        public static bool IsComparerGetHashCodeMethod(this IMethodSymbol method, ITypeSymbol argType)
        {
            return method.Name == nameof(GetHashCode)
                && method.ReturnType.IsInt32()
                && method.Parameters.Length == 1
                && argType.Is(method.Parameters[0].Type);
        }

        public static bool IsComparerEqualsMethod(this IMethodSymbol method, ITypeSymbol argType)
        {
            return method.Name == nameof(Equals)
                && method.ReturnType.IsBoolean()
                && method.Parameters.Length == 2
                && method.Parameters.All(p => argType.Is(p.Type));
        }
    }
}
