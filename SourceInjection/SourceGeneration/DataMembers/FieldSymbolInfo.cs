using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace SourceInjection.SourceGeneration.DataMembers
{
    internal class FieldSymbolInfo : DataMemberSymbolInfo
    {
        protected FieldSymbolInfo(
            string name,
            Microsoft.CodeAnalysis.Accessibility
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

        public static FieldSymbolInfo Create(IFieldSymbol field)
        {
            return new FieldSymbolInfo(
                name: field.Name,
                declaredAccessibility: field.DeclaredAccessibility,
                containingType: field.ContainingType,
                attributes: field.GetAttributes(),
                type: field.Type);
        }
    }
}
