using Aspects.Attributes;
using Aspects.Attributes.Interfaces.Base;
using Aspects.SourceGenerators.Common;
using Aspects.SyntaxReceivers;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators.Base
{
    public abstract class BasicMethodOverrideSourceGeneratorBase<TConfigAttribute, TAttribute, TExcludeAttribute> : TypeSourceGeneratorBase
        where TConfigAttribute : IBasicMethodConfigAttribute
    {
        private protected override TypeSyntaxReceiver SyntaxReceiver { get; } = new TypeSyntaxReceiver(
                Types.WithAttributeOfType<TConfigAttribute>()
            .Or(Types.WithMembersWithAttributeOfType<TAttribute>()));

        private protected override string Dependencies(TypeInfo typeInfo)
        {
            return string.Empty;
        }

        protected private IEnumerable<ISymbol> GetRelevantSymbols(TypeInfo typeInfo)
        {
            return GetSymbols(typeInfo, typeInfo.Members());
        }

        protected private IEnumerable<ISymbol> GetAllSymbols(TypeInfo typeInfo)
        {
            return GetSymbols(typeInfo, typeInfo.Members(true));
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
                .Where(m => m is IFieldSymbol || m is IPropertySymbol p && PropertyIsValid(p))
                .Where(m => m.HasAttributeOfType<TAttribute>());
        }

        protected private bool TryGetDataMemberKind(TypeInfo typeInfo, out DataMemberKind kind)
        {
            var kindTypeName = typeof(DataMemberKind).FullName;

            if(typeInfo.Symbol.GetAttributesOfType<TConfigAttribute>()
                .FirstOrDefault()?.ConstructorArguments
                .SingleOrDefault(arg => arg.Type.ToDisplayString() == kindTypeName) 

                is TypedConstant typedConstant && typedConstant.Value != null)
            {
                kind = (DataMemberKind)typedConstant.Value;

                // TODO: extract out of config
                if (kind == DataMemberKind.ProjectConfig)
                    kind = DataMemberKind.DataMember;

                return true;
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

            fields = fields
                .Where(f => !Array.Exists(linkedFields, lf => lf.Equals(f, SymbolEqualityComparer.Default)))
                .ToArray();

            return fields.Concat(properties.Select(pi => pi.Symbol));
        }

        private IEnumerable<IFieldSymbol> GetFields(IEnumerable<ISymbol> members)
        {
            return members
                .OfType<IFieldSymbol>()
                .Where(f => !f.IsImplicitlyDeclared && !f.IsConst && !f.IsStatic && !f.HasAttributeOfType<TExcludeAttribute>());
        }

        private IEnumerable<IPropertySymbol> GetProperties(IEnumerable<ISymbol> members)
        {
            return members.OfType<IPropertySymbol>()
                .Where(p => PropertyIsValid(p) && (!p.IsOverride || p.HasAttributeOfType<TAttribute>()) && !p.HasAttributeOfType<TExcludeAttribute>());
        }

        private bool PropertyIsValid(IPropertySymbol property)
        {
            return !property.IsImplicitlyDeclared
                && !property.IsStatic
                && property.GetMethod != null
                && !property.GetMethod.IsStatic;
        }
    }
}
