using Microsoft.CodeAnalysis;
using SourceInjection.Util;

namespace SourceInjection.SourceGeneration.DataMembers
{
    internal class AccessorSymbolInfo
    {
        private AccessorSymbolInfo(AccessModifier declaredAccessibility)
        {
            DeclaredAccessibility = declaredAccessibility;
        }

        public AccessModifier DeclaredAccessibility { get; }

        public static AccessorSymbolInfo Create(IMethodSymbol accessor)
        {
            return accessor == null
                ? null
                : new AccessorSymbolInfo(accessor.DeclaredAccessibility.ToAccessModifier());
        }

        public static AccessorSymbolInfo Create(AccessModifier accessibility)
        {
            return new AccessorSymbolInfo(accessibility);
        }
    }
}
