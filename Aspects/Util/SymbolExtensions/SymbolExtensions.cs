using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using Aspects.SourceGeneration.Common;

namespace Aspects.Util.SymbolExtensions
{
    internal static class SymbolExtensions
    {
        public static bool HasAttributeOfType<T>(this ISymbol symbol)
        {
            return symbol.AttributesOfType<T>().Any();
        }

        public static IEnumerable<AttributeData> AttributesOfType<T>(this ISymbol symbol)
        {
            var name = typeof(T).FullName;
            return symbol.GetAttributes().Where(a =>
                a.AttributeClass.Inheritance().Any(ac => ac.ToDisplayString() == name)
                || a.AttributeClass.AllInterfaces.Any(i => i.ToDisplayString() == name));
        }

        public static bool HasDisallowNullAttribute(this ISymbol symbol)
        {
            return symbol.GetAttributes()
                .Any(a => a.AttributeClass.ToDisplayString() == NameOf.DisallowNullAttribute);
        }

        public static bool HasAllowNullAttribute(this ISymbol symbol)
        {
            return symbol.GetAttributes()
                .Any(a => a.AttributeClass.ToDisplayString() == NameOf.AllowNullAttribute);
        }

        public static bool HasNotNullAttribute(this ISymbol symbol)
        {
            return symbol.GetAttributes()
                .Any(a => a.AttributeClass.ToDisplayString() == NameOf.NotNullAttribute);
        }

        public static bool HasMaybeNullAttribute(this ISymbol symbol)
        {
            return symbol.GetAttributes()
                .Any(a => a.AttributeClass.ToDisplayString() == NameOf.MaybeNullAttribute);
        }
    }
}
