using Aspects.Interfaces;
using Aspects.Common;
using Aspects.SourceGeneration.Base;
using Aspects.SourceGeneration.DataMembers;
using Aspects.SourceGeneration.Common;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGeneration.Common.TypeInfo;
using Aspects.Util.SymbolExtensions;

#pragma warning disable IDE0130

namespace Aspects
{
    [Generator]
    internal class SGHashCode : ObjectMethodSourceGeneratorBase<IAutoHashCodeAttribute, IHashCodeAttribute, IHashCodeExcludeAttribute>
    {
        private const string StoredHashCode = "_storedHashCode";

        protected internal override string Name { get; } = nameof(GetHashCode);

        protected override DataMemberPriority Priority { get; } = DataMemberPriority.Field;

        protected override IAutoHashCodeAttribute DefaultConfigAttribute => new AutoHashCodeAttribute();

        protected override IHashCodeAttribute DefaultMemberConfigAttribute => new HashCodeAttribute();

        protected override string TypeBody(TypeInfo typeInfo)
        {
            const int hashCodeCombineMaxArgs = 8;

            var config = GetConfigAttribute(typeInfo);
            var includeBase = ShouldIncludeBase(typeInfo, config);
            var symbols = GetSymbols(typeInfo, typeInfo.Symbol.GetMembers(), config.DataMemberKind);
            var length = symbols.Count + 1 + (includeBase ? 1 : 0);

            var sb = new StringBuilder();

            if (config.StoreHashCode)
            {
                sb.AppendLine($"private int? {StoredHashCode};");
                sb.AppendLine();
            }

            sb.AppendLine($"public override int {nameof(GetHashCode)}()");
            sb.AppendLine("{");

            if (length <= hashCodeCombineMaxArgs)
                sb.AppendLine(HashCodeCombine(typeInfo, config, symbols, includeBase));
            else sb.AppendLine(HashCodeAppend(typeInfo, config, symbols, includeBase));

            sb.Append('}');
            return sb.ToString();
        }

        private string HashCodeAppend(TypeInfo typeInfo, IAutoHashCodeAttribute config, IList<DataMemberSymbolInfo> symbols, bool includeBase)
        {
            var sb = new StringBuilder();
            var name = typeInfo.Symbol.ToDisplayString();

            if (config.StoreHashCode)
            {
                sb.AppendLine(Snippets.Indent($"if({StoredHashCode}.HasValue)"));
                sb.AppendLine(Snippets.Indent($"return {StoredHashCode}.Value;", 2));
            }
            sb.AppendLine(Snippets.Indent("var hash = new System.HashCode();"));
            sb.AppendLine(Snippets.Indent($"hash.Add(\"{name}\");"));

            if (includeBase)
                sb.AppendLine(Snippets.Indent($"hash.Add(base.{nameof(GetHashCode)}());"));

            for (int i = 0; i < symbols.Count; i++)
                sb.AppendLine(Snippets.Indent($"hash.Add({MemberHash(symbols[i], typeInfo)});"));

            if (!config.StoreHashCode)
                sb.Append(Snippets.Indent("return hash.ToHashCode();"));
            else
            {
                sb.AppendLine(Snippets.Indent($"{StoredHashCode} = hash.ToHashCode();"));
                sb.Append(Snippets.Indent($"return {StoredHashCode}.Value;"));
            }
            return sb.ToString();
        }

        private string HashCodeCombine(TypeInfo typeInfo, IAutoHashCodeAttribute config, IList<DataMemberSymbolInfo> symbols, bool includeBase)
        {
            const int tabs = 2;

            var sb = new StringBuilder();
            var name = typeInfo.Symbol.ToDisplayString();

            if (!config.StoreHashCode)
                sb.Append(Snippets.Indent("return"));
            else sb.Append(Snippets.Indent($"{StoredHashCode} ??="));

            sb.Append(" System.HashCode.Combine(");

            sb.AppendLine();
            sb.Append(Snippets.Indent($"\"{name}\"", tabs));
            if (includeBase || symbols.Count > 0)
                sb.AppendLine(",");

            if (includeBase)
            {
                sb.Append(Snippets.Indent($"base.{nameof(GetHashCode)}()", tabs));
                if (symbols.Count > 0)
                    sb.Append(',');
            }

            if (symbols.Count > 0)
            {
                sb.Append(Snippets.Indent($"{MemberHash(symbols[0], typeInfo)}", tabs));

                for (var i = 1; i < symbols.Count; i++)
                {
                    sb.AppendLine(",");
                    sb.Append(Snippets.Indent($"{MemberHash(symbols[i], typeInfo)}", tabs));
                }
            }
            sb.Append(");");
            if (config.StoreHashCode)
            {
                sb.AppendLine();
                sb.Append($"return {StoredHashCode}.Value;");
            }
            return sb.ToString();
        }

        private string MemberHash(DataMemberSymbolInfo member, TypeInfo containingType)
        {
            var memberConfig = GetEqualityConfigAttribute(member);
            var nullSafety = GetNullSafety(member, memberConfig);

            var isNullSafe = nullSafety == NullSafety.On ||
                nullSafety == NullSafety.Auto && (!containingType.HasNullableEnabled || member.Type.HasNullableAnnotation());

            return Snippets.GetHashCode(member, isNullSafe, memberConfig.EqualityComparer);
        }

        private static NullSafety GetNullSafety(DataMemberSymbolInfo member, IEqualityComparisonConfigAttribute memberConfig)
        {
            if (memberConfig.NullSafety != NullSafety.Auto)
                return memberConfig.NullSafety;
            if (member.HasNotNullAttribute())
                return NullSafety.Off;
            if (member.HasMaybeNullAttribute())
                return NullSafety.On;
            if (ComparerSupportsNullSafe(memberConfig.EqualityComparer, member.Type))
                return NullSafety.Off;

            return NullSafety.Auto;
        }

        private IEqualityComparisonConfigAttribute GetEqualityConfigAttribute(DataMemberSymbolInfo member)
        {
            var attribute = member.AttributesOfType<IEqualityComparerAttribute>()
                .FirstOrDefault();

            if (attribute != null && AttributeFactory.TryCreate<IEqualityComparerAttribute>(attribute, out var config))
                return config;
            return GetMemberConfigAttribute(member);
        }

        private static bool ShouldIncludeBase(TypeInfo typeInfo, IAutoHashCodeAttribute config)
        {
            return typeInfo.Symbol.IsReferenceType && (
                config.BaseCall == BaseCall.On
                || config.BaseCall == BaseCall.Auto && typeInfo.Symbol.BaseType is ITypeSymbol syBase && syBase.OverridesGetHashCode());
        }

        private static bool ComparerSupportsNullSafe(string comparer, ITypeSymbol memberType)
        {
            return !string.IsNullOrEmpty(comparer)
                && EqualityComparerInfo.HashCodeSupportsNullable(comparer, memberType);
        }
    }
}

#pragma warning restore IDE0130
