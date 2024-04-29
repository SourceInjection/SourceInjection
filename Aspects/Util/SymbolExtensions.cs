using Aspects.SourceGenerators.Common;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Aspects.Util
{
    internal static class SymbolExtensions
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
            return symbol.ToDisplayString() == CodeSnippets.IEnumerableName
                ||symbol.AllInterfaces.Any(i => i.ToDisplayString() == CodeSnippets.IEnumerableName);
        }

        public static bool OverridesEquals(this ITypeSymbol symbol)
        {
            return symbol.Inheritance().Any(cl => cl.GetMembers().OfType<IMethodSymbol>().Any(m =>
                m.Name == nameof(Equals)
                && m.IsOverride
                && m.ReturnType.ToDisplayString() == "bool"
                && m.Parameters.Length == 1
                && m.Parameters[0].Type.IsTypeOrNullableType("object")));
        }

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

        public static bool IsPublicProperty(this ISymbol symbol)
        {
            return symbol is IPropertySymbol property
                && property.DeclaredAccessibility == Accessibility.Public
                && property.GetMethod != null
                && (property.GetMethod.DeclaredAccessibility == Accessibility.NotApplicable
                    || property.GetMethod.DeclaredAccessibility == Accessibility.Public);
        }

        public static bool IsPublicField(this ISymbol symbol)
        {
            return symbol is IFieldSymbol field
                && field.DeclaredAccessibility == Accessibility.Public;
        }
    }
}
