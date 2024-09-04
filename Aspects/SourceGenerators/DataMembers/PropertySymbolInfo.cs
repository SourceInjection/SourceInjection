using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Common;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Linq;

namespace Aspects.SourceGenerators.Base.DataMembers
{
    internal class PropertySymbolInfo : DataMemberSymbolInfo
    {
        private PropertySymbolInfo(
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

        public bool HidesBasePropertyByName()
        {
            return ContainingType.BaseType?.Inheritance()
                .Any(t => t.GetMembers().OfType<IPropertySymbol>().Any(p => p.Name == Name)) is true;
        }

        public static PropertySymbolInfo Create(IPropertySymbol property)
        {
            return new PropertySymbolInfo(
                name: property.Name,
                declaredAccessibility: property.DeclaredAccessibility,
                containingType: property.ContainingType,
                attributes: property.GetAttributes(),
                type: property.Type,
                generationSource: null);
        }

        public static PropertySymbolInfo Generate(IFieldSymbol field, Accessibility accessibility)
        {
            var name = GetGeneratedPropertyName(field);

            return new PropertySymbolInfo(
                name: name,
                declaredAccessibility: accessibility,
                containingType: field.ContainingType,
                attributes: SelectAttributes(field),
                type: field.Type,
                generationSource: field);
        }

        private static ImmutableArray<AttributeData> SelectAttributes(IFieldSymbol field)
        {
            return field.GetAttributes()
                .Where(a => MustTransfereAttribute(a.AttributeClass))
                .ToImmutableArray();
        }

        private static bool MustTransfereAttribute(INamedTypeSymbol attribute)
        {
            if (attribute == null)
                return false;

            var name = attribute.ToDisplayString();
            return name == NameOf.MaybeNullAttribute
                || name == NameOf.NotNullAttribute
                || attribute.AllInterfaces.Any(i => i.ToDisplayString() == NameOf.IUseOnPropertyGenerationAttribute);
        }

        private static string GetGeneratedPropertyName(IFieldSymbol field)
        {
            var attribute = AttributeFactory.Create<IGeneratesPublicDataMemberPropertyFromFieldAttribute>(
                field.AttributesOfType<IGeneratesPublicDataMemberPropertyFromFieldAttribute>().First());
            return attribute.PropertyName(field);
        }
    }
}
