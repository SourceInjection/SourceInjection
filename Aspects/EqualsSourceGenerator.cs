using Aspects.Attributes;
using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Base;
using Aspects.SourceGenerators.Base.DataMembers;
using Aspects.SourceGenerators.Common;
using Aspects.Util;
using Aspects.Util.SymbolExtensions;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects
{
    [Generator]
    internal class EqualsSourceGenerator
        : ObjectMethodSourceGeneratorBase<IAutoEqualsAttribute, IEqualsAttribute, IEqualsExcludeAttribute>
    {
        private const string argName = "obj";
        private const string otherName = "other";

        protected internal override string Name { get; } = nameof(Equals);

        protected override DataMemberPriority Priority { get; } = DataMemberPriority.Field;

        protected override IAutoEqualsAttribute DefaultConfigAttribute => new AutoEqualsAttribute();

        protected override IEqualsAttribute DefaultMemberConfigAttribute => new EqualsAttribute();

        protected override string TypeBody(TypeInfo typeInfo)
        {
            var sb = new StringBuilder();
            AppendEquals(typeInfo, sb);
            sb.AppendLines(2);
            AppendEquatableEquals(typeInfo, sb);

            return sb.ToString();
        }

        private void AppendEqualsHead(TypeInfo typeInfo, StringBuilder sb, string type, string argName, bool isOverride)
        {
            if (typeInfo.HasNullableEnabled)
                sb.AppendLine("#nullable enable");
            sb.Append("public ");
            if (isOverride)
                sb.Append("override ");
            sb.Append($"bool {nameof(Equals)}({type}");
            if (typeInfo.HasNullableEnabled)
                sb.Append('?');
            sb.AppendLine($" {argName})");
            sb.AppendLine("{");
        }

        private void AppendEquals(TypeInfo typeInfo, StringBuilder sb)
        {
            AppendEqualsHead(typeInfo, sb, "object", argName, true);
            sb.AppendLine(Code.Indent($"return {argName} is {typeInfo.Name} {otherName}"));
            sb.AppendLine(Code.Indent($"&& {nameof(Equals)}({otherName});", 2));
            AppendMethodEnd(typeInfo, sb);
        }

        private void AppendEquatableEquals(TypeInfo typeInfo, StringBuilder sb)
        {
            var config = GetConfigAttribute(typeInfo);
            var symbols = GetSymbols(typeInfo, typeInfo.Symbol.GetMembers(), config.DataMemberKind)
                .ToArray();

            AppendEqualsHead(typeInfo, sb, typeInfo.Name, otherName, false);

            sb.Append(Code.Indent("return "));
            if (typeInfo.Symbol.IsReferenceType)
                sb.Append($"{otherName} == this || ");

            if (typeInfo.Symbol.IsReferenceType || typeInfo.HasNullableEnabled)
                sb.Append($"{otherName} != null");
            else if(symbols.Length == 0)
            {
                sb.AppendLine("true;");
                AppendMethodEnd(typeInfo, sb);
                return;
            }

            if (ShouldIncludeBase(typeInfo, config))
            {
                sb.AppendLine();
                sb.Append(Code.Indent($"&& base.{nameof(Equals)}({otherName})", 2));
            }

            foreach (var symbol in symbols)
            {
                var memberEquals = MemberEquals(
                    symbol,
                    typeInfo.HasNullableEnabled,
                    config);

                sb.AppendLine().Append(Code.Indent($"&& {memberEquals}", 2));
            }

            sb.AppendLine(";");
            AppendMethodEnd(typeInfo, sb);
        }

        private static void AppendMethodEnd(TypeInfo typeInfo, StringBuilder sb)
        {
            sb.Append("}");
            if (typeInfo.HasNullableEnabled)
                sb.AppendLine().Append("#nullable restore");
        }

        private static bool ShouldIncludeBase(TypeInfo typeInfo, IAutoEqualsAttribute configAttribute)
        {
            return typeInfo.Symbol.IsReferenceType && (
                configAttribute.BaseCall == BaseCall.On
                || configAttribute.BaseCall == BaseCall.Auto && typeInfo.Symbol.BaseType is ITypeSymbol syBase && syBase.OverridesEquals());
        }

        private string MemberEquals(DataMemberSymbolInfo symbolInfo, bool nullableEnabled, IAutoEqualsAttribute config)
        {
            var memberConfig = GetEqualityConfigAttribute(symbolInfo);
            var nullSafety = GetNullSafety(symbolInfo, config, memberConfig);

            var type = symbolInfo.Type;
            var nullSafe = nullSafety == NullSafety.On ||
                nullSafety == NullSafety.Auto && (!nullableEnabled || type.HasNullableAnnotation());

            return Comparison(type, symbolInfo.Name, nullSafe , memberConfig.EqualityComparer);
        }

        private IEqualityComparerAttribute GetEqualityConfigAttribute(DataMemberSymbolInfo symbolInfo)
        {
            var attribute = symbolInfo.AttributesOfType<EqualityComparerAttribute>()
                .FirstOrDefault();

            if (attribute != null && AttributeFactory.TryCreate<EqualityComparerAttribute>(attribute, out var config))
                return config;
            return GetMemberConfigAttribute(symbolInfo);
        }

        private static NullSafety GetNullSafety(DataMemberSymbolInfo symbol, IAutoEqualsAttribute config, IEqualityComparerAttribute memberConfig)
        {
            if (memberConfig.NullSafety != NullSafety.Auto)
                return memberConfig.NullSafety;
            if (symbol.HasNotNullAttribute())
                return NullSafety.Off;
            if (symbol.HasMaybeNullAttribute())
                return NullSafety.On;
            return config.NullSafety;
        }

        private static string Comparison(ITypeSymbol type, string memberName, bool nullSafe, string comparer)
        {
            var snippet = Code.EqualityCheck(type, memberName, $"{otherName}.{memberName}", nullSafe, comparer);
            return snippet.Contains("||")
                ? $"({snippet})"
                : snippet;
        }
    }
}
