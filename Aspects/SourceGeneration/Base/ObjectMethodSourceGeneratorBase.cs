using Aspects.Interfaces;
using Aspects.SourceGeneration.Base.DataMembers;
using Aspects.SourceGeneration.DataMembers;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using TypeInfo = Aspects.SourceGeneration.Common.TypeInfo;
using PropertyInfo = Aspects.Common.PropertyInfo;
using Aspects.Util.SymbolExtensions;
using Aspects.Common;
using Aspects.SourceGeneration.SyntaxReceivers;

namespace Aspects.SourceGeneration.Base
{
    internal abstract class ObjectMethodSourceGeneratorBase<TConfigAttribute, TAttribute, TExcludeAttribute> : TypeSourceGeneratorBase 
        where TConfigAttribute : class 
        where TAttribute : class
        where TExcludeAttribute : class
    {
        protected enum DataMemberPriority { Field, Property };

        protected abstract TConfigAttribute DefaultConfigAttribute { get; }

        protected abstract TAttribute DefaultMemberConfigAttribute { get; }

        protected override TypeSyntaxReceiver SyntaxReceiver { get; } = new TypeSyntaxReceiver(
                TypeInfo.WithAttributeOfType<TConfigAttribute>()
            .Or(TypeInfo.WithMembersWithAttributeOfType<TAttribute>()));

        protected abstract DataMemberPriority Priority { get; }

        protected override IEnumerable<string> Dependencies(TypeInfo typeInfo) => Enumerable.Empty<string>();

        protected IList<DataMemberSymbolInfo> GetSymbols(TypeInfo typeInfo, IEnumerable<ISymbol> members, DataMemberKind dataMemberKind)
        {
            var result = new List<DataMemberSymbolInfo>(32);
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
                    result.Add(PropertySymbolInfo.Create(p));
                else if(member is IFieldSymbol f)
                {
                    if (MustUseGeneratedProperty(f, dataMemberKind))
                        result.Add(GeneratedPropertySymbolInfo.Create(f, Accessibility.Public));
                    else if (IsTargeted(f, dataMemberKind))
                        result.Add(FieldSymbolInfo.Create(f));
                }
            }
            return result;
        }

        private bool MustUseGeneratedProperty(IFieldSymbol f, DataMemberKind dataMemberKind)
        {
            return f.HasAttributeOfType<IGeneratesPublicDataMemberPropertyFromFieldAttribute>()
                && (dataMemberKind == DataMemberKind.DataMember && Priority == DataMemberPriority.Property || dataMemberKind == DataMemberKind.Property);
        }

        private static bool IsTargeted(IFieldSymbol field, DataMemberKind dataMemberKind)
        {
            return field.IsInstanceMember() 
                && !field.HasAttributeOfType<TExcludeAttribute>() 
                && (dataMemberKind != DataMemberKind.Property 
                    && !field.IsImplicitlyDeclared 
                    || field.HasAttributeOfType<TAttribute>());
        }

        private static bool IsTargeted(IPropertySymbol property, DataMemberKind dataMemberKind, IEnumerable<PropertyInfo> propertyInfos)
        {
            return property.IsInstanceMember() 
                && property.GetMethod != null
                && !property.IsIndexer
                && !property.HasAttributeOfType<TExcludeAttribute>() 
                && (dataMemberKind != DataMemberKind.Field
                    && !property.IsImplicitlyDeclared 
                    && !property.IsOverride 
                    && IsDataMember(property, propertyInfos)
                    || property.HasAttributeOfType<TAttribute>());
        }

        private static bool IsDataMember(IPropertySymbol property, IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.FirstOrDefault(
                pi => pi.Symbol.Equals(property, SymbolEqualityComparer.Default))?.IsDataMember == true;
        }

        protected TAttribute GetMemberConfigAttribute(DataMemberSymbolInfo symbol)
        {
            return GetFirstOrNull<TAttribute>(symbol.AttributesOfType<TAttribute>())
                ?? DefaultMemberConfigAttribute;
        }

        protected TConfigAttribute GetConfigAttribute(TypeInfo typeInfo)
        {
            return GetFirstOrNull<TConfigAttribute>(typeInfo.Symbol.AttributesOfType<TConfigAttribute>())
                ?? DefaultConfigAttribute;
        }

        private static T GetFirstOrNull<T>(IEnumerable<AttributeData> attributes) where T : class
        {
            var attData = attributes.FirstOrDefault();
            if (attData is null)
                return null;
            return AttributeFactory.Create<T>(attData);
        }
    }
}