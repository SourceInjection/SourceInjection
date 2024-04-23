using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Aspects.Util
{
    internal static class SymbolExtensions
    {
        public static bool HasAttributeOfType<T>(this ISymbol symbol)
        {
            return symbol.GetAttributesOfType<T>().Any();
        }

        public static IEnumerable<AttributeData> GetAttributesOfType<T>(this ISymbol symbol)
        {
            var name = typeof(T).FullName;
            return symbol.GetAttributes().Where(a =>
                a.AttributeClass.Inheritance().Any(ac => ac.ToDisplayString() == name)
                || a.AttributeClass.AllInterfaces.Any(i => i.ToDisplayString() == name));
        }

        public static IEnumerable<INamedTypeSymbol> Inheritance(this INamedTypeSymbol symbol)
        {
            while(symbol != null)
            {
                yield return symbol;
                symbol = symbol.BaseType;
            }
        }
    }
}
