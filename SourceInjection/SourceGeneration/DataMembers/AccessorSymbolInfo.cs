using Microsoft.CodeAnalysis;
using SourceInjection.Util;

namespace SourceInjection.SourceGeneration.DataMembers
{
    internal class AccessorSymbolInfo
    {
        private AccessorSymbolInfo(AccessModifier declaredModifier)
        {
            DeclaredModifier = declaredModifier;
        }

        public AccessModifier DeclaredModifier { get; }

        public static AccessorSymbolInfo Create(IMethodSymbol accessor)
        {
            return accessor == null
                ? null
                : new AccessorSymbolInfo(accessor.DeclaredAccessibility.ToAccessModifier());
        }

        public static AccessorSymbolInfo Create(AccessModifier modifier)
        {
            return new AccessorSymbolInfo(modifier);
        }
    }
}
