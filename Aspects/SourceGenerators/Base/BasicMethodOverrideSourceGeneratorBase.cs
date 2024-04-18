using Aspects.Attributes;
using Aspects.Attributes.Base;
using Aspects.SourceGenerators.Common;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators.Base
{
    public abstract class BasicMethodOverrideSourceGeneratorBase : TypeSourceGeneratorBase 
    {
        protected abstract ISet<string> TypeAttributes { get; }

        protected abstract ISet<string> ExcludeAttributes { get; }

        private protected override string Dependencies(TypeInfo typeInfo)
        {
            return string.Empty;
        }

        protected private IEnumerable<ISymbol> GetDeclaredSymbols(TypeInfo typeInfo)
        {
            return GetSymbols(typeInfo, typeInfo.Members());
        }

        protected private IEnumerable<ISymbol> GetAllSymbols(TypeInfo typeInfo)
        {
            return GetSymbols(typeInfo, typeInfo.Members(true));
        }

        private IEnumerable<ISymbol> GetSymbols(TypeInfo typeInfo, IEnumerable<ISymbol> members)
        {
            var attribute = GetAttribute(typeInfo);

            if (attribute != null)
            {
                if (attribute.DataMemberKind == DataMemberKind.Property)
                    return GetProperties(members);
                else if (attribute.DataMemberKind == DataMemberKind.Field)
                    return GetFields(members);
                else if (attribute.DataMemberKind == DataMemberKind.DataMember)
                    return GetDataMembers(members);
                throw new NotImplementedException();
            }

            return members
                .Where(m => m is IFieldSymbol || m is IPropertySymbol p && PropertyIsValid(p))
                .Where(m => MemberHasTypeAttribute(m));
        }

        protected private BasicOverrideMethodAttribute GetAttribute(TypeInfo typeInfo)
        {
            return BasicOverrideMethodAttribute.FromAttributeData(typeInfo.Symbol.GetAttributes()
                .First(a => TypeAttributes.Contains(a.AttributeClass.ToDisplayString())));
        }

        private bool MemberHasTypeAttribute(ISymbol m)
        {
            var attributeNames = m.GetAttributes()
                .Select(a => a.AttributeClass.ToDisplayString());

            return attributeNames.Any(a => TypeAttributes.Contains(a))
                && !attributeNames.Any(a => ExcludeAttributes.Contains(a));
        }

        private IEnumerable<ISymbol> GetDataMembers(IEnumerable<ISymbol> members)
        {
            IEnumerable<ISymbol> fields = GetFields(members);
            var properties = GetProperties(members).ToArray();
            fields = fields.Where(f => !Array.Exists(properties, p => p.Name == Property.Name(f.Name))).ToArray();
            return fields.Concat(properties);
        }

        private IEnumerable<IPropertySymbol> GetProperties(IEnumerable<ISymbol> members)
        {
            return members.OfType<IPropertySymbol>()
                .Where(p => PropertyIsValid(p) && !p.HasAnyAttribute(ExcludeAttributes));
        }

        private static bool PropertyIsValid(IPropertySymbol property)
        {
            return !property.IsOverride
                && !property.IsImplicitlyDeclared
                && !property.IsStatic
                && property.GetMethod != null
                && !property.GetMethod.IsStatic;
        }

        private IEnumerable<IFieldSymbol> GetFields(IEnumerable<ISymbol> members)
        {
            return members
                .OfType<IFieldSymbol>()
                .Where(f => !f.IsImplicitlyDeclared && !f.IsConst && !f.IsStatic && !f.HasAnyAttribute(ExcludeAttributes));
        } 
    }
}
