using SourceInjection.CodeAnalysis;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace SourceInjection.SourceGeneration.SyntaxReceivers
{
    internal static class Types
    {
        public static Predicate<INamedTypeSymbol> WithAttributeOfType<T>()
        {
            return (symbol) => symbol.HasAttributeOfType<T>();
        }

        public static Predicate<INamedTypeSymbol> WithMembersWithAttributeOfType<T>()
        {
            return (symbol) => symbol.GetMembers().Any(m => m.HasAttributeOfType<T>());
        }
    }
}
