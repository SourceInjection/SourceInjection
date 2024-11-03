using SourceInjection.Interfaces;
using SourceInjection.SourceGeneration.Base;
using SourceInjection.SourceGeneration.DataMembers;
using SourceInjection.SourceGeneration.Common;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Text;
using TypeInfo = SourceInjection.SourceGeneration.Common.TypeInfo;
using SourceInjection.SourceGeneration;
using SourceInjection.CodeAnalysis;
using SourceInjection.Util;

#pragma warning disable IDE0130

namespace SourceInjection
{
    [Generator(LanguageNames.CSharp)]
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
                sb.AppendLine(Text.Indent($"if({StoredHashCode}.HasValue)"));
                sb.AppendLine(Text.Indent($"return {StoredHashCode}.Value;", 2));
            }
            sb.AppendLine(Text.Indent("var hash = new System.HashCode();"));
            sb.AppendLine(Text.Indent($"hash.Add(\"{name}\");"));

            if (includeBase)
                sb.AppendLine(Text.Indent($"hash.Add(base.{nameof(GetHashCode)}());"));

            for (int i = 0; i < symbols.Count; i++)
                sb.AppendLine(Text.Indent($"hash.Add({MemberHash(symbols[i], typeInfo)});"));

            if (!config.StoreHashCode)
                sb.Append(Text.Indent("return hash.ToHashCode();"));
            else
            {
                sb.AppendLine(Text.Indent($"{StoredHashCode} = hash.ToHashCode();"));
                sb.Append(Text.Indent($"return {StoredHashCode}.Value;"));
            }
            return sb.ToString();
        }

        private string HashCodeCombine(TypeInfo typeInfo, IAutoHashCodeAttribute config, IList<DataMemberSymbolInfo> symbols, bool includeBase)
        {
            const int tabs = 2;

            var sb = new StringBuilder();
            var name = typeInfo.Symbol.ToDisplayString();

            if (!config.StoreHashCode)
                sb.Append(Text.Indent("return"));
            else sb.Append(Text.Indent($"{StoredHashCode} ??="));

            sb.Append(" System.HashCode.Combine(");

            sb.AppendLine();
            sb.Append(Text.Indent($"\"{name}\"", tabs));
            if (includeBase || symbols.Count > 0)
                sb.AppendLine(",");

            if (includeBase)
            {
                sb.Append(Text.Indent($"base.{nameof(GetHashCode)}()", tabs));
                if (symbols.Count > 0)
                    sb.Append(',');
            }

            if (symbols.Count > 0)
            {
                sb.Append(Text.Indent($"{MemberHash(symbols[0], typeInfo)}", tabs));

                for (var i = 1; i < symbols.Count; i++)
                {
                    sb.AppendLine(",");
                    sb.Append(Text.Indent($"{MemberHash(symbols[i], typeInfo)}", tabs));
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
            var comparerConfig = GetComparerAttribute(member);
            var comparerInfo = EqualityComparerInfo.Get(comparerConfig?.EqualityComparer, member.Type);

            var nullSafety = GetNullSafety(member, comparerConfig, comparerInfo);

            var isNullSafe = nullSafety == NullSafety.On ||
                nullSafety == NullSafety.Auto && (!containingType.HasNullableEnabled || member.Type.HasNullableAnnotation());

            return Snippets.GetHashCode(member, isNullSafe, comparerInfo);
        }

        private NullSafety GetNullSafety(DataMemberSymbolInfo member, IEqualityComparerAttribute comparerConfig, EqualityComparerInfo comparerInfo)
        {
            if (comparerConfig != null && comparerConfig.NullSafety != NullSafety.Auto)
                return comparerConfig.NullSafety;
            
            if (member.HasNotNullAttribute())
                return NullSafety.Off;
            if (member.HasMaybeNullAttribute())
                return NullSafety.On;

            if (comparerInfo != null)
                return comparerInfo.HashCodeSupportsNullable ? NullSafety.Off : NullSafety.On;

            return NullSafety.Auto;
        }

        private static bool ShouldIncludeBase(TypeInfo typeInfo, IAutoHashCodeAttribute config)
        {
            return typeInfo.Symbol.IsReferenceType && (
                config.BaseCall == BaseCall.On
                || config.BaseCall == BaseCall.Auto && typeInfo.Symbol.BaseType is ITypeSymbol syBase && syBase.OverridesGetHashCode());
        }
    }
}

#pragma warning restore IDE0130
