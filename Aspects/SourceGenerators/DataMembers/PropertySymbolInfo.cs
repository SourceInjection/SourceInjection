using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Aspects.SourceGenerators.Base.DataMembers
{
    internal class PropertySymbolInfo : DataMemberSymbolInfo
    {
        protected PropertySymbolInfo(
            string name,
            Accessibility
            declaredAccessibility,
            ITypeSymbol containingType,
            ImmutableArray<AttributeData> attributes,
            ITypeSymbol type

            ) : base(

            name: name,
            declaredAccessibility: declaredAccessibility,
            containingType: containingType,
            attributes: attributes,
            type: type)
        { }

        public static PropertySymbolInfo Create(IPropertySymbol property)
        {
            return new PropertySymbolInfo(
                name: property.Name,
                declaredAccessibility: property.DeclaredAccessibility,
                containingType: property.ContainingType,
                attributes: property.GetAttributes(),
                type: property.Type);
        }
    }
}
