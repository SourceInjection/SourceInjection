using SourceInjection.Interfaces;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;
using SourceInjection.SourceGeneration.Common;
using SourceInjection.CodeAnalysis;

namespace SourceInjection.SourceGeneration.DataMembers
{
    internal class GeneratedPropertySymbolInfo : PropertySymbolInfo
    {
        protected GeneratedPropertySymbolInfo(
            string name,
            AccessModifier declaredAccessibility,
            ITypeSymbol containingType,
            ImmutableArray<AttributeData> attributes,
            ITypeSymbol type,
            IFieldSymbol generationSource,
            AccessorSymbolInfo getAccessor,
            AccessorSymbolInfo setAccessor

            ) : base(

            name: name,
            declaredAccessibility: declaredAccessibility,
            containingType: containingType,
            attributes: attributes,
            type: type,
            getAccessor: getAccessor,
            setAccessor: setAccessor)
        { 
            GenerationSource = generationSource;
        }

        public IFieldSymbol GenerationSource { get; }


        public override bool HasMaybeNullAttribute()
        {
            return GenerationSource.HasMaybeNullAttribute();
        }

        public static GeneratedPropertySymbolInfo Create(IFieldSymbol field)
        {
            var attribute = AttributeFactory.Create<IGeneratesDataMemberPropertyFromFieldAttribute>(
                field.AttributesOfType<IGeneratesDataMemberPropertyFromFieldAttribute>().First());

            return new GeneratedPropertySymbolInfo(
                name: attribute.PropertyName(field),
                declaredAccessibility: attribute.Accessibility,
                containingType: field.ContainingType,
                attributes: field.GetAttributes(),
                type: field.Type,
                generationSource: field,
                getAccessor: AccessorSymbolInfo.Create(attribute.GetterAccessibility),
                setAccessor: null);
        }
    }
}
