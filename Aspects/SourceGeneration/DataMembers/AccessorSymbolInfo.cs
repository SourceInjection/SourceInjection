
using Microsoft.CodeAnalysis;

namespace Aspects.SourceGeneration.DataMembers
{
    internal class AccessorSymbolInfo
    {
        private AccessorSymbolInfo(Accessibility declaredAccessibility)
        {
            DeclaredAccessibility = declaredAccessibility;
        }

        public Accessibility DeclaredAccessibility { get; }

        public static AccessorSymbolInfo Create(IMethodSymbol accessor)
        {
            return accessor == null
                ? null
                : new AccessorSymbolInfo(accessor.DeclaredAccessibility);
        }

        public static AccessorSymbolInfo Create(Accessibility accessibility)
        {
            return new AccessorSymbolInfo(accessibility);
        }
    }
}
