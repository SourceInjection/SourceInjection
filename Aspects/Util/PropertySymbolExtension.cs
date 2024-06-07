using Microsoft.CodeAnalysis;
using System.Linq;

namespace Aspects.Util
{
    internal static class PropertySymbolExtension
    {
        /// <summary>
        /// Checks if a property hides a base property by name.
        /// </summary>
        /// <param name="symbol">The <see cref="IPropertySymbol"/> which is checked.</param>
        /// <returns><see langword="true"/> if any base property is hidden else <see langword="false"/>.</returns>
        public static bool HidesBasePropertyByName(this IPropertySymbol symbol)
        {
            var type = symbol.ContainingType;
            if (type is null)
                return false;

            return type.Inheritance()
                .Any(t => t.GetMembers().OfType<IPropertySymbol>().Any(p => p.Name == symbol.Name));
        }
    }
}
