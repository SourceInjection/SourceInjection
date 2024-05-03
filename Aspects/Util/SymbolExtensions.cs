using Aspects.SourceGenerators.Common;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

namespace Aspects.Util
{
    internal static class SymbolExtensions
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
        /// Checks if the <see cref="ISymbol"/> has an <see cref="Attribute"/> of type T.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="symbol">The <see cref="ISymbol"/> which is checked if it has an <see cref="Attribute"/> of type T.</param>
        /// <returns>true if the <see cref="ISymbol"/> has an <see cref="Attribute"/> of type T else false.</returns>
        public static bool HasAttributeOfType<T>(this ISymbol symbol)
        {
            return symbol.AttributesOfType<T>().Any();
        }

        /// <summary>
        /// Gets all <see cref="AttributeData"/> of type T within a <see cref="ISymbol"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="symbol">The <see cref="ISymbol"/> for which the <see cref="AttributeData"/> are evaluated.</param>
        /// <returns>A <see cref="IEnumerable"/> of <see cref="AttributeData"/> where the associated <see cref="Attribute"/> is of type T.</returns>
        public static IEnumerable<AttributeData> AttributesOfType<T>(this ISymbol symbol)
        {
            var name = typeof(T).FullName;
            return symbol.GetAttributes().Where(a =>
                a.AttributeClass.Inheritance().Any(ac => ac.ToDisplayString() == name)
                || a.AttributeClass.AllInterfaces.Any(i => i.ToDisplayString() == name));
        }

        /// <summary>
        /// Gets all local visible <see cref="IFieldSymbol"/>s within a <see cref="INamedTypeSymbol"/>.
        /// </summary>
        /// <param name="symbol">The <see cref="INamedTypeSymbol"/> for which the <see cref="IFieldSymbol"/>s are evaluated.</param>
        /// <returns>A <see cref="IEnumerable"/> of the local visible <see cref="IFieldSymbol"/>s.</returns>
        public static IEnumerable<IFieldSymbol> LocalVisibleFields(this INamedTypeSymbol symbol)
        {
            var result = symbol.GetMembers().OfType<IFieldSymbol>();
            if(symbol.BaseType != null)
            {
                result = result.Concat(
                    symbol.BaseType.Inheritance()
                        .SelectMany(sy => sy.GetMembers().OfType<IFieldSymbol>())
                        .Where(f => IsVisibleFromDerived(symbol, f)));
            }
            return result;
        }

        /// <summary>
        /// Checks if a property hides a base property by name.
        /// </summary>
        /// <param name="symbol">The <see cref="IPropertySymbol"/> which is checked.</param>
        /// <returns>true if any base property is hidden else false.</returns>
        public static bool HidesBasePropertyByName(this IPropertySymbol symbol)
        {
            var type = symbol.ContainingType;
            if (type is null)
                return false;

            return type.Inheritance()
                .Any(t => t.GetMembers().OfType<IPropertySymbol>().Any(p => p.Name == symbol.Name));
        }

        /// <summary>
        /// Computes the inheritance of a <see cref="ITypeSymbol"/> (including itself).
        /// </summary>
        /// <param name="symbol">The <see cref="ITypeSymbol"/> for which the inheritance is computed.</param>
        /// <returns>A <see cref="IEnumerable"/> of <see cref="ITypeSymbol"/>s representing the inheritance from top to bottom.</returns>
        public static IEnumerable<ITypeSymbol> Inheritance(this ITypeSymbol symbol)
        {
            while(symbol != null)
            {
                yield return symbol;
                symbol = symbol.BaseType;
            }
        }

        /// <summary>
        /// Checks if the symbol implements the given interface.
        /// </summary>
        /// <typeparam name="T">The interface type to be checked.</typeparam>
        /// <param name="symbol">The symbol for wich the interface implementation is checked.</param>
        /// <returns>true if the symbol implements the given interface else false</returns>
        public static bool Implements<T>(this ITypeSymbol symbol)
        {
            var name = typeof(T).FullName;
            return symbol.AllInterfaces.Any(i => i.ToDisplayString() == name);
        }

        /// <summary>
        /// Checks if the <see cref="ITypeSymbol"/> implements the interface <see cref="IEnumerable"/>.
        /// </summary>
        /// <param name="symbol">The <see cref="ITypeSymbol"/> which is checked to be a <see cref="IEnumerable"/>.</param>
        /// <returns>true if the <see cref="ITypeSymbol"/> is a <see cref="IEnumerable"/> else false</returns>
        public static bool IsEnumerable(this ITypeSymbol symbol)
        {
            return symbol.ToDisplayString() == CodeSnippets.IEnumerableName
                ||symbol.AllInterfaces.Any(i => i.ToDisplayString() == CodeSnippets.IEnumerableName);
        }


        /// <summary>
        /// Checks if the <see cref="ITypeSymbol"/> overrides <see cref="object.Equals(object)"/> itself or in any base class.
        /// </summary>
        /// <param name="symbol">the <see cref="ITypeSymbol"/> which is checked to override <see cref="object.Equals(object)"/>.</param>
        /// <returns>true if the <see cref="object.Equals(object)"/> method is overridden else false.</returns>
        public static bool OverridesEquals(this ITypeSymbol symbol)
        {
            return symbol.Inheritance().Any(cl => cl.GetMembers().OfType<IMethodSymbol>().Any(m =>
                m.Name == nameof(Equals)
                && m.IsOverride
                && m.ReturnType.ToDisplayString() == "bool"
                && m.Parameters.Length == 1
                && m.Parameters[0].Type.IsTypeOrNullableType("object")));
        }

        /// <summary>
        /// Checks if the <see cref="ITypeSymbol"/> overrides <see cref="object.GetHashCode"/> itself or in any base class.
        /// </summary>
        /// <param name="symbol">the <see cref="ITypeSymbol"/> which is checked to override <see cref="object.GetHashCode"/>.</param>
        /// <returns>true if the <see cref="object.GetHashCode"/> method is overridden else false</returns>
        public static bool OverridesGetHashCode(this ITypeSymbol symbol)
        {
            return symbol.Inheritance().Any(cl => cl.GetMembers().OfType<IMethodSymbol>().Any(m =>
                m.Name == nameof(GetHashCode)
                && m.IsOverride
                && m.ReturnType.ToDisplayString() == "int"));
        }

        /// <summary>
        /// Checks if the <see cref="ISymbol"/> is a public <see cref="IPropertySymbol"/>.
        /// </summary>
        /// <param name="symbol">The symbol which is checked to be a public <see cref="IPropertySymbol".</param>
        /// <returns>true if the <see cref="ISymbol"/> is a public <see cref="IPropertySymbol"/> else false.</returns>
        public static bool IsPublicProperty(this ISymbol symbol)
        {
            return symbol is IPropertySymbol property
                && property.DeclaredAccessibility == Accessibility.Public
                && property.GetMethod != null
                && (property.GetMethod.DeclaredAccessibility == Accessibility.NotApplicable
                    || property.GetMethod.DeclaredAccessibility == Accessibility.Public);
        }

        /// <summary>
        /// Checks if the given <see cref="ISymbol"/> is a public <see cref="IFieldSymbol"/>.
        /// </summary>
        /// <param name="symbol">The <see cref="ISymbol"/> which is checked to be a public <see cref="IFieldSymbol"/>.</param>
        /// <returns>true if the <see cref="ISymbol"/> is a public <see cref="IFieldSymbol"/> else false.</returns>
        public static bool IsPublicField(this ISymbol symbol)
        {
            return symbol is IFieldSymbol field
                && field.DeclaredAccessibility == Accessibility.Public;
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

        private static bool IsTypeOrNullableType(this ITypeSymbol symbol, string typeName)
        {
            var s = symbol.ToDisplayString();
            return s == typeName
                || s == typeName + '?';
        }
    }
}
