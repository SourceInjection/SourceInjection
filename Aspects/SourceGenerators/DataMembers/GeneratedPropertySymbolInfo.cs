using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Base.DataMembers;
using Aspects.SourceGenerators.Common;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;

namespace Aspects.SourceGenerators.DataMembers
{
    internal class GeneratedPropertySymbolInfo : PropertySymbolInfo
    {
        protected GeneratedPropertySymbolInfo(
            string name,
            Accessibility
            declaredAccessibility,
            ITypeSymbol containingType,
            ImmutableArray<AttributeData> attributes,
            ITypeSymbol type,
            IFieldSymbol generationSource

            ) : base(

            name: name,
            declaredAccessibility: declaredAccessibility,
            containingType: containingType,
            attributes: attributes,
            type: type)
        { 
            GenerationSource = generationSource;
        }

        public IFieldSymbol GenerationSource { get; }


        public override bool HasMaybeNullAttribute()
        {
            return GenerationSource.HasMaybeNullAttribute();
        }

        public static GeneratedPropertySymbolInfo Create(IFieldSymbol field, Accessibility accessibility)
        {
            var name = GetGeneratedPropertyName(field);

            return new GeneratedPropertySymbolInfo(
                name: name,
                declaredAccessibility: accessibility,
                containingType: field.ContainingType,
                attributes: field.GetAttributes(),
                type: field.Type,
                generationSource: field);
        }

        private static string GetGeneratedPropertyName(IFieldSymbol field)
        {
            var attribute = AttributeFactory.Create<IGeneratesPublicDataMemberPropertyFromFieldAttribute>(
                field.AttributesOfType<IGeneratesPublicDataMemberPropertyFromFieldAttribute>().First());
            return attribute.PropertyName(field);
        }
    }
}
