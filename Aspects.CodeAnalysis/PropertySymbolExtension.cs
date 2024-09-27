using Microsoft.CodeAnalysis;
using System.Linq;

namespace Aspects.CodeAnalysis
{
    public static class PropertySymbolExtension
    {
        public static bool HidesBasePropertyByName(this IPropertySymbol symbol)
        {
            var type = symbol.ContainingType;
            if (type is null)
                return false;

            return type.Inheritance()
                .Any(t => t.GetMembers().OfType<IPropertySymbol>().Any(p => p.Name == symbol.Name));
        }

        public static bool IsInstanceMember(this IPropertySymbol property)
        {
            return !property.IsStatic
                && (property.GetMethod == null || !property.GetMethod.IsStatic)
                && (property.SetMethod == null || !property.SetMethod.IsStatic);
        }
    }
}
