using Microsoft.CodeAnalysis;
using SourceInjection.Util;
using System.Collections.Immutable;

namespace SourceInjection.SourceGeneration.DataMembers
{
    internal class PropertySymbolInfo : DataMemberSymbolInfo
    {
        protected PropertySymbolInfo(
            string name,
            AccessModifier declaredModifier,
            ITypeSymbol containingType,
            ImmutableArray<AttributeData> attributes,
            ITypeSymbol type,
            AccessorSymbolInfo getAccessor,
            AccessorSymbolInfo setAccessor

            ) : base(

            name: name,
            declaredModifier: declaredModifier,
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
                declaredModifier: property.DeclaredAccessibility.ToAccessModifier(),
                containingType: property.ContainingType,
                attributes: property.GetAttributes(),
                type: property.Type,
                getAccessor: AccessorSymbolInfo.Create(property.GetMethod),
                setAccessor: AccessorSymbolInfo.Create(property.SetMethod));
        }
    }
}
