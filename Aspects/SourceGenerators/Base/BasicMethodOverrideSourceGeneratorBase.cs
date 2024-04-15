using Aspects.Attributes;
using Aspects.Attributes.Base;
using Aspects.SourceGenerators.Common;
using Aspects.SourceGenerators.SyntaxReceivers;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators.Base
{
    public abstract class BasicMethodOverrideSourceGeneratorBase<TTypeAttribute, TExcludeAttribute> : TypeSourceGeneratorBase 
        where TTypeAttribute : BaseOverrideMethodAttribute 
        where TExcludeAttribute : Attribute
    {
        private static readonly string s_attributeName = typeof(TTypeAttribute).FullName;
        private static readonly string s_excludeName = typeof(TExcludeAttribute).FullName;

        private protected override TypeSyntaxReceiver SyntaxReceiver { get; }
            = new TypeSyntaxReceiver(Types.With<HashCodeAttribute>());

        protected private abstract TTypeAttribute ConvertToAttribute(AttributeData attributeData);

        private protected override string Dependencies(TypeInfo typeInfo)
        {
            return string.Empty;
        }

        protected private IEnumerable<ISymbol> GetSymbols(TypeInfo typeInfo)
        {
            var attribute = GetAttribute(typeInfo);

            if (attribute.DataMemberKind == DataMemberKind.Properties)
                return GetProperties(typeInfo, attribute);
            else if (attribute.DataMemberKind == DataMemberKind.Fields)
                return GetFields(typeInfo, attribute);
            else if (attribute.DataMemberKind == DataMemberKind.All)
                return GetDataMembers(typeInfo, attribute);
            else if (attribute.DataMemberKind == DataMemberKind.PublicProperties)
                return GetProperties(typeInfo, attribute).Where(p => p.DeclaredAccessibility == Accessibility.Public);
            else if (attribute.DataMemberKind == DataMemberKind.PublicFields)
                return GetFields(typeInfo, attribute).Where(f => f.DeclaredAccessibility == Accessibility.Public);
            else if (attribute.DataMemberKind == DataMemberKind.Public)
                return GetDataMembers(typeInfo, attribute).Where(sy => sy.DeclaredAccessibility == Accessibility.Public);

            throw new NotImplementedException();
        }

        private static IEnumerable<ISymbol> GetDataMembers(TypeInfo typeInfo, TTypeAttribute attribute)
        {
            IEnumerable<ISymbol> fields = GetFields(typeInfo, attribute).ToArray();

            var excludedPropNames = fields
                .Select(f => Property.Name(f.Name))
                .ToImmutableHashSet();

            var properties = GetProperties(typeInfo, attribute)
                .Where(p => !excludedPropNames.Contains(p.Name));

            return fields.Concat(properties);
        }

        protected private TTypeAttribute GetAttribute(TypeInfo typeInfo)
        {
            return ConvertToAttribute(typeInfo.Symbol.GetAttributes()
                .First(a => a.AttributeClass.ToDisplayString() == s_attributeName));
        }

        private static IEnumerable<IPropertySymbol> GetProperties(TypeInfo typeInfo, TTypeAttribute attribute)
        {
            return typeInfo.Members
                .OfType<IPropertySymbol>()
                .Where(p => !attribute.ExcludedMembers.Contains(p.Name) && GetterIsValid(p) 
                    && !p.GetAttributes().Any(a => a.AttributeClass.ToDisplayString() == s_excludeName));
        }

        private static bool GetterIsValid(IPropertySymbol property)
        {
            return !property.IsStatic
                && property.GetMethod != null
                && !property.GetMethod.IsStatic;
        }

        private static IEnumerable<IFieldSymbol> GetFields(TypeInfo typeInfo, TTypeAttribute attribute)
        {
            return typeInfo.Members
                .OfType<IFieldSymbol>()
                .Where(f => !attribute.ExcludedMembers.Contains(f.Name) && !f.IsImplicitlyDeclared && !f.IsConst && !f.IsStatic
                    && !f.GetAttributes().Any(a => a.AttributeClass.ToDisplayString() == s_excludeName));
        } 
    }
}
