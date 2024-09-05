using Aspects.Attributes;
using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Base.DataMembers;
using Aspects.SourceGenerators.Common;
using Aspects.SourceGenerators.DataMembers;
using Aspects.SourceGenerators.Queries;
using Aspects.SyntaxReceivers;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators.Base
{
    internal abstract class ObjectMethodSourceGeneratorBase<TConfigAttribute, TAttribute, TExcludeAttribute> : TypeSourceGeneratorBase
    {
        protected enum DataMemberPriority { Field, Property };

        protected override TypeSyntaxReceiver SyntaxReceiver { get; } = new TypeSyntaxReceiver(
                Types.WithAttributeOfType<TConfigAttribute>()
            .Or(Types.WithMembersWithAttributeOfType<TAttribute>()));

        protected abstract DataMemberPriority Priority { get; }

        protected override IEnumerable<string> Dependencies(TypeInfo typeInfo) => Enumerable.Empty<string>();

        protected IEnumerable<DataMemberSymbolInfo> GetSymbols(TypeInfo typeInfo, IEnumerable<ISymbol> members, DataMemberKind dataMemberKind)
        {
            var conflicts = typeInfo.LocalPropertyInfos
                .Where(pi => pi.LinkedField != null
                    && IsTargeted(pi.LinkedField, dataMemberKind)
                    && IsTargeted(pi.Symbol, dataMemberKind, typeInfo.LocalPropertyInfos));

            var blockedSymbols = Priority == DataMemberPriority.Field
                ? new HashSet<ISymbol>(conflicts.Select(pi => pi.Symbol), SymbolEqualityComparer.Default)
                : new HashSet<ISymbol>(conflicts.Select(pi => pi.LinkedField), SymbolEqualityComparer.Default);

            foreach (var member in members.Where(m => !blockedSymbols.Contains(m, SymbolEqualityComparer.Default)))
            {
                if (member is IPropertySymbol p && IsTargeted(p, dataMemberKind, typeInfo.LocalPropertyInfos))
                    yield return PropertySymbolInfo.Create(p);
                else if(member is IFieldSymbol f)
                {
                    if (MustUseGeneratedProperty(f, dataMemberKind))
                        yield return GeneratedPropertySymbolInfo.Create(f, Accessibility.Public);
                    else if (IsTargeted(f, dataMemberKind))
                        yield return FieldSymbolInfo.Create(f);
                }
            }
        }

        private bool MustUseGeneratedProperty(IFieldSymbol f, DataMemberKind dataMemberKind)
        {
            return f.HasAttributeOfType<IGeneratesPublicDataMemberPropertyFromFieldAttribute>()
                && (dataMemberKind == DataMemberKind.DataMember && Priority == DataMemberPriority.Property || dataMemberKind == DataMemberKind.Property);
        }

        private static bool IsTargeted(IFieldSymbol field, DataMemberKind dataMemberKind)
        {
            return !field.HasAttributeOfType<TExcludeAttribute>() && (
                    dataMemberKind != DataMemberKind.Property
                    && !field.IsImplicitlyDeclared 
                    && !field.IsConst 
                    && !field.IsStatic
                || field.HasAttributeOfType<TAttribute>());
        }

        private static bool IsTargeted(IPropertySymbol property, DataMemberKind dataMemberKind, IEnumerable<PropertyInfo> propertyInfos)
        {
            return !property.HasAttributeOfType<TExcludeAttribute>() && (
                    dataMemberKind != DataMemberKind.Field
                    && !property.IsImplicitlyDeclared
                    && !property.IsStatic
                    && property.GetMethod != null
                    && !property.GetMethod.IsStatic
                    && !property.IsOverride
                    && IsDataMember(property, propertyInfos)
                || property.HasAttributeOfType<TAttribute>());
        }

        private static bool IsDataMember(IPropertySymbol property, IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.FirstOrDefault(
                pi => pi.Symbol.Equals(property, SymbolEqualityComparer.Default))?.IsDataMember == true;
        }
    }
}