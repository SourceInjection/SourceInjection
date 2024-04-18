using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Aspects.Util
{
    internal static class SymbolExtensions
    {
        public static bool HasAttribute(this ISymbol symbol, string fullName)
        {
            return symbol.GetAttributes().Any(a => a.AttributeClass.ToDisplayString() == fullName);
        }

        public static bool HasAnyAttribute(this ISymbol symbol, ISet<string> attributeFullNames)
        {
            return symbol.GetAttributes().Any(a => attributeFullNames.Contains(a.AttributeClass.ToDisplayString()));
        }

        public static bool HasAnyAttribute(this ISymbol symbol, IEnumerable<string> attributeFullNames)
        {
            return symbol.GetAttributes().Any(a => attributeFullNames.Contains(a.AttributeClass.ToDisplayString()));
        }
    }
}
