using Microsoft.CodeAnalysis;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aspects.Util
{
    internal static class SymbolExtensions
    {
        private static readonly string EnumerableName = typeof(IEnumerable).FullName;
        private static readonly string BooleanName = typeof(bool).FullName;

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

        public static IEnumerable<ITypeSymbol> Inheritance(this ITypeSymbol symbol)
        {
            while(symbol != null)
            {
                yield return symbol;
                symbol = symbol.BaseType;
            }
        }

        public static bool IsEnumerable(this ITypeSymbol symbol)
        {
            return symbol.ToDisplayString() == EnumerableName
                ||symbol.AllInterfaces.Any(i => i.ToDisplayString() == EnumerableName);
        }

        public static bool OverridesEquals(this ITypeSymbol symbol)
        {
            return symbol.Inheritance().Any(cl => cl.GetMembers().OfType<IMethodSymbol>().Any(m =>
                m.Name == "Equals"
                && m.IsOverride
                && m.ReturnType.ToDisplayString() == "bool"
                && m.Parameters.Length == 1
                && m.Parameters[0].Type.IsTypeOrNullableType("object")));
        }

        private static bool IsTypeOrNullableType(this ITypeSymbol symbol, string typeName)
        {
            var s = symbol.ToDisplayString();
            return s == typeName
                || s == $"{typeName}?";
        }
    }
}
