
using Microsoft.CodeAnalysis;

namespace Aspects.SourceGeneration.DataMembers
{
    internal class AccessorSymbolInfo
    {
        private AccessorSymbolInfo(Microsoft.CodeAnalysis.Accessibility declaredAccessibility)
        {
            DeclaredAccessibility = declaredAccessibility;
        }

        public Microsoft.CodeAnalysis.Accessibility DeclaredAccessibility { get; }

        public static AccessorSymbolInfo Create(IMethodSymbol accessor)
        {
            return accessor == null
                ? null
                : new AccessorSymbolInfo(accessor.DeclaredAccessibility);
        }

        public static AccessorSymbolInfo Create(Microsoft.CodeAnalysis.Accessibility accessibility)
        {
            return new AccessorSymbolInfo(accessibility);
        }
    }
}
