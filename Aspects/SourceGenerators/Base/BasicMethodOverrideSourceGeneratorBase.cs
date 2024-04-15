using Aspects.Attributes;
using Aspects.Attributes.Base;
using Aspects.SourceGenerators.Common;
using Aspects.SourceGenerators.SyntaxReceivers;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators.Base
{
    public abstract class BasicMethodOverrideSourceGeneratorBase<TTypeAttribute, TExcludeAttribute> : TypeSourceGeneratorBase 
        where TTypeAttribute : BasicOverrideMethodAttribute 
        where TExcludeAttribute : Attribute
    {
        private static readonly string s_attributeName = typeof(TTypeAttribute).FullName;
        private static readonly string s_excludeName = typeof(TExcludeAttribute).FullName;

        private protected override TypeSyntaxReceiver SyntaxReceiver { get; }
            = new TypeSyntaxReceiver(Types.With<TTypeAttribute>());

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

        private static IEnumerable<ISymbol> GetDataMembers(TypeInfo typeInfo, BasicOverrideMethodAttribute attribute)
        {
            IEnumerable<ISymbol> fields = GetFields(typeInfo, attribute);
            var properties = GetProperties(typeInfo, attribute).ToArray();
            fields = fields.Where(f => !Array.Exists(properties, p => p.Name == Property.Name(f.Name))).ToArray();
            return fields.Concat(properties);
        }

        protected private BasicOverrideMethodAttribute GetAttribute(TypeInfo typeInfo)
        {
            return BasicOverrideMethodAttribute.FromAttributeData(typeInfo.Symbol.GetAttributes()
                .First(a => a.AttributeClass.ToDisplayString() == s_attributeName));
        }

        private static IEnumerable<IPropertySymbol> GetProperties(TypeInfo typeInfo, BasicOverrideMethodAttribute attribute)
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

        private static IEnumerable<IFieldSymbol> GetFields(TypeInfo typeInfo, BasicOverrideMethodAttribute attribute)
        {
            return typeInfo.Members
                .OfType<IFieldSymbol>()
                .Where(f => !attribute.ExcludedMembers.Contains(f.Name) && !f.IsImplicitlyDeclared && !f.IsConst && !f.IsStatic
                    && !f.GetAttributes().Any(a => a.AttributeClass.ToDisplayString() == s_excludeName));
        } 
    }
}
