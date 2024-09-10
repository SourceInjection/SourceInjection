using Aspects.Attributes;
using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Base;
using Aspects.SourceGenerators.Base.DataMembers;
using Aspects.SourceGenerators.Common;
using Aspects.Util;
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
            var symbols = GetSymbols(typeInfo, typeInfo.Symbol.GetMembers(), config.DataMemberKind);

            AppendEqualsHead(typeInfo, sb, typeInfo.Name, otherName, false);

            sb.Append(Code.Indent("return "));
            if (typeInfo.Symbol.IsReferenceType)
                sb.Append($"{otherName} == this || ");

            if (typeInfo.Symbol.IsReferenceType || typeInfo.HasNullableEnabled)
                sb.Append($"{otherName} != null");
            else if(symbols.Count == 0)
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
                    typeInfo,
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

        private string MemberEquals(DataMemberSymbolInfo member, TypeInfo containingType, IAutoEqualsAttribute config)
        {
            var memberConfig = GetEqualityConfigAttribute(member);
            var nullSafety = GetNullSafety(member, config, memberConfig);

            var isNullSafe = nullSafety == NullSafety.On ||
                nullSafety == NullSafety.Auto && (!containingType.HasNullableEnabled || member.Type.HasNullableAnnotation());

            var snippet = Code.EqualityCheck(member, $"{otherName}.{member.Name}", isNullSafe, memberConfig.EqualityComparer);

            return snippet.Contains("||")
                ? $"({snippet})"
                : snippet;
        }

        private IEqualityComparisonConfigAttribute GetEqualityConfigAttribute(DataMemberSymbolInfo symbolInfo)
        {
            var attribute = symbolInfo.AttributesOfType<IEqualityComparerAttribute>()
                .FirstOrDefault();

            if (attribute != null && AttributeFactory.TryCreate<IEqualityComparerAttribute>(attribute, out var config))
                return config;
            return GetMemberConfigAttribute(symbolInfo);
        }

        private static NullSafety GetNullSafety(DataMemberSymbolInfo member, IAutoEqualsAttribute config, IEqualityComparisonConfigAttribute memberConfig)
        {
            if (memberConfig.NullSafety != NullSafety.Auto)
                return memberConfig.NullSafety;
            if (member.HasNotNullAttribute())
                return NullSafety.Off;
            if (member.HasMaybeNullAttribute())
                return NullSafety.On;
            if(config.NullSafety == NullSafety.Auto && !string.IsNullOrEmpty(memberConfig.EqualityComparer))
            {
                if (new EqualityComparerInfo(memberConfig.EqualityComparer, member.Type).EqualsSupportsNullable)
                    return NullSafety.Off;
                return NullSafety.On;
            }
            return config.NullSafety;
        }
    }
}
