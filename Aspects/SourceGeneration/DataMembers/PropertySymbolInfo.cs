using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace Aspects.SourceGeneration.DataMembers
{
    internal class PropertySymbolInfo : DataMemberSymbolInfo
    {
        protected PropertySymbolInfo(
            string name,
            Microsoft.CodeAnalysis.Accessibility declaredAccessibility,
            ITypeSymbol containingType,
            ImmutableArray<AttributeData> attributes,
            ITypeSymbol type,
            AccessorSymbolInfo getAccessor,
            AccessorSymbolInfo setAccessor

            ) : base(

            name: name,
            declaredAccessibility: declaredAccessibility,
            containingType: containingType,
            attributes: attributes,
            type: type)
        {
            GetAccessor = getAccessor;
            SetAccessor = setAccessor;
        }

        public AccessorSymbolInfo GetAccessor { get; }

        public AccessorSymbolInfo SetAccessor { get; }

        public static PropertySymbolInfo Create(IPropertySymbol property)
        {
            return new PropertySymbolInfo(
                name: property.Name,
                declaredAccessibility: property.DeclaredAccessibility,
                containingType: property.ContainingType,
                attributes: property.GetAttributes(),
                type: property.Type,
                getAccessor: AccessorSymbolInfo.Create(property.GetMethod),
                setAccessor: AccessorSymbolInfo.Create(property.SetMethod));
        }
    }
}
