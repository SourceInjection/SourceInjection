using Aspects.Interfaces;
using Microsoft.CodeAnalysis;
using Aspects.CodeAnalysis;
using System.Linq;

namespace Aspects.Util
{
    internal static class TypeSymbolExtensions
    {
        public static bool OverridesToString(this ITypeSymbol symbol)
        {
            return symbol.HasAttributeOfType<IAutoToStringAttribute>()
                || symbol.GetMembers().Any(m => m.HasAttributeOfType<IToStringAttribute>())
                || symbol.GetMembers().OfType<IMethodSymbol>().Any(m =>
                    m.Name == nameof(ToString)
                    && m.IsOverride
                    && m.ReturnType.IsString()
                    && m.Parameters.Length == 0);
        }

        public static bool OverridesEquals(this ITypeSymbol symbol)
        {
            return symbol.HasAttributeOfType<IAutoEqualsAttribute>()
                || symbol.GetMembers().Any(m => m.HasAttributeOfType<IEqualsAttribute>())
                || symbol.GetMembers().OfType<IMethodSymbol>().Any(m =>
                    m.Name == nameof(Equals)
                    && m.IsOverride
                    && m.ReturnType.IsBoolean()
                    && m.Parameters.Length == 1
                    && m.Parameters[0].Type.IsObject(true));
        }

        public static bool OverridesGetHashCode(this ITypeSymbol symbol)
        {
            return symbol.HasAttributeOfType<IAutoHashCodeAttribute>()
                || symbol.GetMembers().Any(m => m.HasAttributeOfType<IHashCodeAttribute>())
                || symbol.Inheritance().Any(cl => cl.GetMembers().OfType<IMethodSymbol>().Any(m =>
                    m.Name == nameof(GetHashCode)
                    && m.IsOverride
                    && m.Parameters.Length == 0
                    && m.ReturnType.IsInt32()));
        }

        public static bool CanUseSequenceEquals(this ITypeSymbol type)
        {
            if (type is IArrayTypeSymbol ats)
                return ats.Rank == 1 && CanUseObjectMethods(ats.ElementType);

            return type is INamedTypeSymbol nts
                && type.IsKnownGenericCollection()
                && nts.TypeArguments.All(ta => CanUseObjectMethods(ta));
        }

        public static bool CanUseCombinedHashCode(this ITypeSymbol type)
        {
            if (type is IArrayTypeSymbol ats)
                return CanUseObjectMethods(ats.ElementType);

            return type is INamedTypeSymbol nts
                && type.IsKnownGenericCollection()
                && nts.TypeArguments.All(ta => CanUseObjectMethods(ta));
        }

        private static bool CanUseObjectMethods(ITypeSymbol type)
        {
            return !type.IsEnumerable()
                || type.CanUseEqualityOperatorsByDefault()
                || type.OverridesEquals();
        }

        private static bool IsKnownGenericCollection(this ITypeSymbol type)
        {
            var name = type.ToDisplayString().TrimEnd('?');
            if (name.Contains('<') && name.Contains('>'))
                name = name.Substring(0, name.IndexOf('<') + 1) + name.Substring(name.LastIndexOf('>'));
            return KnownTypes.GenericCollections.Contains(name);
        }
    }
}
