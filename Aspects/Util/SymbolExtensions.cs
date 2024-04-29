using Aspects.SourceGenerators.Common;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Aspects.Util
{
    internal static class SymbolExtensions
    {
        /// <summary>
        /// Computes the inheritance ordered from bottom to top of a type
        /// </summary>
        /// <param name="symbol">The symbol for wich the inheritance is evaluated</param>
        /// <returns>A collection that represents the inheritance ordered from bottom to top</returns>
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
        /// Checks if the symbol has an attribute of type T
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <param name="symbol">The symbol which is checked if it has an attribute of type T</param>
        /// <returns>true if the symbol has an attribute of type T else false</returns>
        public static bool HasAttributeOfType<T>(this ISymbol symbol)
        {
            return symbol.AttributesOfType<T>().Any();
        }

        /// <summary>
        /// Gets all attributes of type T placed over a symbol
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <param name="symbol">The symbol for which the attributes are evaluated</param>
        /// <returns>A collection of attributes of type T</returns>
        public static IEnumerable<AttributeData> AttributesOfType<T>(this ISymbol symbol)
        {
            var name = typeof(T).FullName;
            return symbol.GetAttributes().Where(a =>
                a.AttributeClass.Inheritance().Any(ac => ac.ToDisplayString() == name)
                || a.AttributeClass.AllInterfaces.Any(i => i.ToDisplayString() == name));
        }

        /// <summary>
        /// Gets all local visible fields within a type
        /// </summary>
        /// <param name="symbol">The named type symbol for which the fields are evaluated</param>
        /// <returns>A collection of field symbols</returns>
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
        /// Checks if a property hides a base property by name
        /// </summary>
        /// <param name="symbol">The property symbol which is checked</param>
        /// <returns>true if any base property is hidden else false</returns>
        public static bool HidesBasePropertyByName(this IPropertySymbol symbol)
        {
            var type = symbol.ContainingType;
            if (type is null)
                return false;

            return type.Inheritance()
                .Any(t => t.GetMembers().OfType<IPropertySymbol>().Any(p => p.Name == symbol.Name));
        }

        private static bool IsVisibleFromDerived(INamedTypeSymbol symbol, IFieldSymbol member)
        {
            if (member.DeclaredAccessibility == Accessibility.Public 
                || member.DeclaredAccessibility == Accessibility.Protected 
                || member.DeclaredAccessibility == Accessibility.ProtectedOrInternal)
            {
                return true;
            }

            if(member.DeclaredAccessibility == Accessibility.ProtectedAndInternal)
            {
                var memberAssebly = member.ContainingType?.ContainingAssembly;
                if (memberAssebly is null)
                    return false;
                return symbol.ContainingAssembly.Equals(memberAssebly, SymbolEqualityComparer.Default);
            }

            return false;
        }

        /// <summary>
        /// Computes the inheritance of a type symbol (including itself)
        /// </summary>
        /// <param name="symbol">The type symbol for which the inheritance is computed</param>
        /// <returns>A collection of type symbols representing the inheritance from top to bottom</returns>
        public static IEnumerable<ITypeSymbol> Inheritance(this ITypeSymbol symbol)
        {
            while(symbol != null)
            {
                yield return symbol;
                symbol = symbol.BaseType;
            }
        }

        /// <summary>
        /// Checks if the type symbol implements the interface System.Collections.IEnumerable
        /// </summary>
        /// <param name="symbol">The type symbol which is checked to be a collection</param>
        /// <returns>true if the symbol is a collection else false</returns>
        public static bool IsEnumerable(this ITypeSymbol symbol)
        {
            return symbol.ToDisplayString() == CodeSnippets.IEnumerableName
                ||symbol.AllInterfaces.Any(i => i.ToDisplayString() == CodeSnippets.IEnumerableName);
        }

        /// <summary>
        /// Checks if the type overrides Equals itself or in any base class
        /// </summary>
        /// <param name="symbol">the type symbol which is checked to override Equals</param>
        /// <returns>true if the Equals method is overridden else false</returns>
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
        /// Checks if the type overrides GetHashCode itself or in any base class
        /// </summary>
        /// <param name="symbol">the type symbol which is checked to override GetHashCode</param>
        /// <returns>true if the GetHashCode method is overridden else false</returns>
        public static bool OverridesGetHashCode(this ITypeSymbol symbol)
        {
            return symbol.Inheritance().Any(cl => cl.GetMembers().OfType<IMethodSymbol>().Any(m =>
                m.Name == nameof(GetHashCode)
                && m.IsOverride
                && m.ReturnType.ToDisplayString() == "int"));
        }

        private static bool IsTypeOrNullableType(this ITypeSymbol symbol, string typeName)
        {
            var s = symbol.ToDisplayString();
            return s == typeName
                || s == typeName + '?';
        }

        /// <summary>
        /// Checks if the given symbol is a public property (IPropertySymbol)
        /// </summary>
        /// <param name="symbol">The symbol which is checked to be a public property</param>
        /// <returns>true if the symbol is a public property else false</returns>
        public static bool IsPublicProperty(this ISymbol symbol)
        {
            return symbol is IPropertySymbol property
                && property.DeclaredAccessibility == Accessibility.Public
                && property.GetMethod != null
                && (property.GetMethod.DeclaredAccessibility == Accessibility.NotApplicable
                    || property.GetMethod.DeclaredAccessibility == Accessibility.Public);
        }

        /// <summary>
        /// Checks if the given symbol is a public field (IFieldSymbol)
        /// </summary>
        /// <param name="symbol">The symbol which is checked to be a public field</param>
        /// <returns>true if the symbol is a public field else false</returns>
        public static bool IsPublicField(this ISymbol symbol)
        {
            return symbol is IFieldSymbol field
                && field.DeclaredAccessibility == Accessibility.Public;
        }
    }
}
