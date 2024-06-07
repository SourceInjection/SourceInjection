using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Aspects.Util
{
    internal static class NamedTypeSymbolExtensions
    {
        /// <summary>
        /// Computes the inheritance ordered from bottom to top of a <see cref="INamedTypeSymbol"/>.
        /// </summary>
        /// <param name="symbol">The <see cref="INamedTypeSymbol"/> for wich the inheritance is evaluated.</param>
        /// <returns>A <see cref="IEnumerable"/> that represents the inheritance ordered from bottom to top.</returns>
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

        /// <summary>
        /// Gets all local visible <see cref="IFieldSymbol"/>s within a <see cref="INamedTypeSymbol"/>.
        /// </summary>
        /// <param name="symbol">The <see cref="INamedTypeSymbol"/> for which the <see cref="IFieldSymbol"/>s are evaluated.</param>
        /// <returns>A <see cref="IEnumerable"/> of the local visible <see cref="IFieldSymbol"/>s.</returns>
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

        private static bool IsVisibleFromDerived(INamedTypeSymbol symbol, IFieldSymbol member)
        {
            if (member.DeclaredAccessibility == Accessibility.Public
                || member.DeclaredAccessibility == Accessibility.Protected
                || member.DeclaredAccessibility == Accessibility.ProtectedOrInternal)
            {
                return true;
            }

            if (member.DeclaredAccessibility == Accessibility.ProtectedAndInternal)
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
