using Aspects.Attributes;
using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Queries;
using Aspects.SyntaxReceivers;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators.Base
{
    internal abstract class BasicMethodOverrideSourceGeneratorBase<TConfigAttribute, TAttribute, TExcludeAttribute> 
        : TypeSourceGeneratorBase
        where TConfigAttribute : IBasicMethodConfigAttribute
    {
        protected enum DataMemberPriority { Field, Property };

        protected override TypeSyntaxReceiver SyntaxReceiver { get; } = new TypeSyntaxReceiver(
                Types.WithAttributeOfType<TConfigAttribute>()
            .Or(Types.WithMembersWithAttributeOfType<TAttribute>()));

        protected abstract DataMemberPriority Priority { get; }

        protected override string Dependencies(TypeInfo typeInfo)
        {
            return string.Empty;
        }

        protected IEnumerable<ISymbol> GetLocalTargetedSymbols(TypeInfo typeInfo)
        {
            return GetSymbols(typeInfo, typeInfo.Members());
        }

        protected IEnumerable<ISymbol> GetPublicSymbols(TypeInfo typeInfo)
        {
            return GetSymbols(typeInfo, typeInfo.Members(true).Where(sy => sy.IsPublicProperty() || sy.IsPublicField()));
        }

        private IEnumerable<ISymbol> GetSymbols(TypeInfo typeInfo, IEnumerable<ISymbol> members)
        {
            if (TryGetDataMemberKind(typeInfo, out var dataMemberKind))
            {
                if (dataMemberKind == DataMemberKind.Property)
                    return GetProperties(members);
                else if (dataMemberKind == DataMemberKind.Field)
                    return GetFields(members);
                else if (dataMemberKind == DataMemberKind.DataMember)
                    return GetDataMembers(typeInfo, members);
                throw new NotImplementedException();
            }

            return members
                .Where(m => m is IFieldSymbol || m is IPropertySymbol p && PropertyIsInstanceMember(p))
                .Where(m => m.HasAttributeOfType<TAttribute>());
        }

        private bool TryGetDataMemberKind(TypeInfo typeInfo, out DataMemberKind kind)
        {
            if (typeInfo.Symbol.GetAttributesOfType<TConfigAttribute>().FirstOrDefault() is AttributeData attData)
            {
                var att = Common.Attribute.Create<TConfigAttribute>(attData);
                if(att?.DataMemberKind is DataMemberKind dk)
                {
                    kind = dk;
                    // TODO: extract out of config
                    if (kind == DataMemberKind.ProjectConfig)
                        kind = DataMemberKind.DataMember;

                    return true;
                }
            }

            kind = DataMemberKind.DataMember;
            return false;
        }

        private IEnumerable<ISymbol> GetDataMembers(TypeInfo typeInfo, IEnumerable<ISymbol> members)
        {
            IEnumerable<ISymbol> fields = GetFields(members);

            var properties = typeInfo.LocalProperties
                .Where(pi => pi.IsDataMember);

            var linkedFields = properties
                .Where(pi => pi.LinkedField != null)
                .Select(pi => pi.LinkedField)
                .ToArray();

            if (Priority == DataMemberPriority.Property)
            {
                fields = fields
                    .Where(f => !Array.Exists(linkedFields, lf => lf.Equals(f, SymbolEqualityComparer.Default)))
                    .ToArray();

                return fields.Concat(properties.Select(pi => pi.Symbol));
            }
            else if (Priority == DataMemberPriority.Field)
            {
                return fields.Concat(properties
                    .Where(pi => pi.LinkedField is null)
                    .Select(pi => pi.Symbol));
            }
            else throw new NotImplementedException($"{Priority}");
        }

        private IEnumerable<IFieldSymbol> GetFields(IEnumerable<ISymbol> members)
        {
            return members
                .OfType<IFieldSymbol>()
                .Where(f => !f.IsImplicitlyDeclared && !f.IsConst && !f.IsStatic && !f.HasAttributeOfType<TExcludeAttribute>());
        }

        private IEnumerable<IPropertySymbol> GetProperties(IEnumerable<ISymbol> members)
        {
            return members.OfType<IPropertySymbol>().Where(p => 
                PropertyIsInstanceMember(p) 
                && (!p.IsOverride || p.HasAttributeOfType<TAttribute>()) && !p.HasAttributeOfType<TExcludeAttribute>());
        }

        private bool PropertyIsInstanceMember(IPropertySymbol property)
        {
            return !property.IsImplicitlyDeclared
                && !property.IsStatic
                && property.GetMethod != null
                && !property.GetMethod.IsStatic;
        }
    }
}
