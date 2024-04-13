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

            if (attribute.DataMemberKind == DataMemberKind.Property)
                return GetProperties(typeInfo);
            else if (attribute.DataMemberKind == DataMemberKind.Field)
                return GetFields(typeInfo);

            IEnumerable<ISymbol> fields = GetFields(typeInfo).ToArray();

            var excludedPropNames = fields
                .Select(f => Property.Name(f.Name))
                .ToImmutableHashSet();

            var properties = GetProperties(typeInfo)
                .Where(p => !excludedPropNames.Contains(p.Name));

            return fields.Concat(properties);
        }

        private TTypeAttribute GetAttribute(TypeInfo typeInfo)
        {
            return ConvertToAttribute(typeInfo.Symbol.GetAttributes().First(a => a.AttributeClass.ToDisplayString() == s_attributeName));
        }

        private static IEnumerable<IPropertySymbol> GetProperties(TypeInfo typeInfo)
        {
            return typeInfo.Members
                .OfType<IPropertySymbol>()
                .Where(p => GetterIsValid(p) && !p.GetAttributes().Any(a => a.AttributeClass.ToDisplayString() == s_excludeName));
        }

        private static bool GetterIsValid(IPropertySymbol property)
        {
            return !property.IsAbstract
                && !property.IsStatic
                && property.GetMethod != null
                && !property.GetMethod.IsAbstract
                && !property.GetMethod.IsStatic
                && property.GetMethod.DeclaredAccessibility == Accessibility.Public;
        }

        private static IEnumerable<IFieldSymbol> GetFields(TypeInfo typeInfo)
        {
            return typeInfo.Members
                .OfType<IFieldSymbol>()
                .Where(f => !f.IsImplicitlyDeclared && !f.IsConst && !f.IsStatic && !f.IsAbstract 
                    && !f.GetAttributes().Any(a => a.AttributeClass.ToDisplayString() == s_excludeName));
        } 
    }
}
