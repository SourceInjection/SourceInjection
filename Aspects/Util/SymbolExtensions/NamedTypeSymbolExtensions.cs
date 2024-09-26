using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Aspects.Util.SymbolExtensions
{
    internal static class NamedTypeSymbolExtensions
    {
        public static IEnumerable<INamedTypeSymbol> InheritanceFromBottomToTop(this INamedTypeSymbol symbol)
        {
            var stack = new Stack<INamedTypeSymbol>();

            while (symbol != null)
            {
                stack.Push(symbol);
                symbol = symbol.BaseType;
            }

            while (stack.Count > 0)
                yield return stack.Pop();
        }

        public static IEnumerable<IFieldSymbol> LocalVisibleFields(this INamedTypeSymbol symbol)
        {
            var result = symbol.GetMembers().OfType<IFieldSymbol>();
            if (symbol.BaseType != null)
            {
                result = result.Concat(
                    symbol.BaseType.Inheritance()
                        .SelectMany(sy => sy.GetMembers().OfType<IFieldSymbol>())
                        .Where(f => IsVisibleFromDerived(symbol, f)));
            }
            return result;
        }

        public static IEnumerable<ISymbol> GetAllMembers(this INamedTypeSymbol symbol)
        {
            return symbol.Inheritance().SelectMany(sy => sy.GetMembers());
        }

        private static bool IsVisibleFromDerived(INamedTypeSymbol symbol, IFieldSymbol member)
        {
            if (member.DeclaredAccessibility == Microsoft.CodeAnalysis.Accessibility.Public
                || member.DeclaredAccessibility == Microsoft.CodeAnalysis.Accessibility.Protected
                || member.DeclaredAccessibility == Microsoft.CodeAnalysis.Accessibility.ProtectedOrInternal)
            {
                return true;
            }

            if (member.DeclaredAccessibility == Microsoft.CodeAnalysis.Accessibility.ProtectedAndInternal)
            {
                var memberAssebly = member.ContainingType?.ContainingAssembly;
                if (memberAssebly is null)
                    return false;
                return symbol.ContainingAssembly.Equals(memberAssebly, SymbolEqualityComparer.Default);
            }

            return false;
        }
    }
}
