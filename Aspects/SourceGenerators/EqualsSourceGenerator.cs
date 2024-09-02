using Aspects.Attributes;
using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Base;
using Aspects.SourceGenerators.Common;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators
{
    [Generator]
    internal class EqualsSourceGenerator
        : ObjectMethodSourceGeneratorBase<IAutoEqualsAttribute, IEqualsAttribute, IEqualsExcludeAttribute>
    {
        private const string argName = "obj";
        private const string otherName = "other";

        protected internal override string Name { get; } = nameof(Equals);

        protected override DataMemberPriority Priority { get; } = DataMemberPriority.Field;

        protected override DataMemberKind DataMemberKindFromAttribute(IAutoEqualsAttribute attr)
        {
            return attr.DataMemberKind;
        }

        protected override string TypeBody(TypeInfo typeInfo)
        {
            var config = GetConfigAttribute(typeInfo);

            var sb = new StringBuilder();
            AppendMethodStart(typeInfo, sb);

            sb.Append(CodeSnippets.Indent("return"));
            if (typeInfo.Symbol.IsReferenceType)
                sb.Append($" {argName} == this ||");

            sb.Append($" {argName} is {typeInfo.Name}");

            var symbols = GetSymbols(typeInfo, config);
            if (symbols.Length > 0)
                sb.Append($" {otherName}");

            if (ShouldIncludeBase(typeInfo, config))
            {
                sb.AppendLine();
                sb.Append(CodeSnippets.Indent($"&& base.{Name}({argName})", 2));
            }

            foreach (var symbol in symbols)
            {
                var memberEquals = MemberEquals(
                    symbol,
                    typeInfo.HasNullableEnabled,
                    config);

                sb.AppendLine().Append(CodeSnippets.Indent($"&& {memberEquals}", 2));
            }

            sb.AppendLine(";");

            AppendMethodEnd(typeInfo, sb);
            return sb.ToString();
        }

        private ISymbol[] GetSymbols(TypeInfo typeInfo, IAutoEqualsAttribute config)
        {
            var symbols = GetLocalTargetedSymbols(typeInfo);
            if (config.DataMemberKind == DataMemberKind.Property)
            {
                symbols = symbols.Concat(typeInfo.Symbol.GetMembers()
                    .OfType<IFieldSymbol>()
                    .Where(f => f.HasAttributeOfType<IGeneratesPublicDataMemberPropertyFromFieldAttribute>()));
            }
            return symbols.ToArray();
        }

        private void AppendMethodStart(TypeInfo typeInfo, StringBuilder sb)
        {
            if (typeInfo.HasNullableEnabled)
                sb.AppendLine("#nullable enable");
            sb.Append($"public override bool {Name}(object");
            if (typeInfo.HasNullableEnabled)
                sb.Append('?');
            sb.AppendLine($" {argName})");
            sb.AppendLine("{");
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
                configAttribute.BaseCall == BaseCall.On || configAttribute.BaseCall == BaseCall.Auto 
                    && typeInfo.Symbol.BaseType is ITypeSymbol syBase && (
                        syBase.HasAttributeOfType<IAutoEqualsAttribute>() 
                        || syBase.OverridesEquals() 
                        || syBase.GetMembers().Any(m => m.HasAttributeOfType<IEqualsAttribute>())));
        }

        private static NullSafety GetNullSafety(ISymbol symbol, IAutoEqualsAttribute config, IEqualsAttribute memberConfig)
        {
            if (memberConfig.NullSafety != NullSafety.Auto)
                return memberConfig.NullSafety;
            if (symbol.HasNotNullAttribute())
                return NullSafety.Off;
            if(symbol.HasMaybeNullAttribute())
                return NullSafety.On;
            return config.NullSafety;
        }

        private static string MemberEquals(ISymbol symbol, bool nullableEnabled, IAutoEqualsAttribute config)
        {
            var memberConfig = GetMemberAttribute(symbol);
            var nullSafety = GetNullSafety(symbol, config, memberConfig);

            var type = GetType(symbol);
            var nullSafe = nullSafety == NullSafety.On ||
                nullSafety == NullSafety.Auto && (!nullableEnabled || type.HasNullableAnnotation());

            var memberName = GetMemberName(symbol, config);

            if (string.IsNullOrEmpty(memberConfig.EqualityComparer))
                return Comparison(type, memberName, nullSafe);

            return ComparerComparison(memberConfig.EqualityComparer, memberName, nullSafe && type.IsReferenceType);
        }

        private static string ComparerComparison(string comparer, string memberName, bool nullSafe)
        {
            var s = $"new {comparer}().Equals({memberName}, {otherName}.{memberName})";
            if (nullSafe)
                s = $"({memberName} == null && {otherName}.{memberName} == null || {memberName} != null && {otherName}.{memberName} != null && {s})";
            return s;
        }

        private static string GetMemberName(ISymbol symbol, IAutoEqualsAttribute config)
        {
            if (!MustUsePropertyInstead(symbol, config))
                return symbol.Name;

            var field = (IFieldSymbol)symbol;
            var propName = field.AttributesOfType<IGeneratesPublicDataMemberPropertyFromFieldAttribute>()
                .Select(a => AttributeFactory.TryCreate<IGeneratesPublicDataMemberPropertyFromFieldAttribute>(a, out var attr) ? attr : null)
                .Select(a => a?.PropertyName(field))
                .FirstOrDefault(s => !string.IsNullOrEmpty(s));

            return propName ?? symbol.Name;
        }

        private static bool MustUsePropertyInstead(ISymbol symbol, IAutoEqualsAttribute config)
        {
            return config.DataMemberKind == DataMemberKind.Property
                && symbol is IFieldSymbol
                && symbol.HasAttributeOfType<IGeneratesPublicDataMemberPropertyFromFieldAttribute>();
        }

        private static ITypeSymbol GetType(ISymbol symbol)
        {
            if (symbol is IFieldSymbol field)
                return field.Type;
            if (symbol is IPropertySymbol property)
                return property.Type;
            throw new NotImplementedException();
        }

        private static string Comparison(ITypeSymbol type, string memberName, bool nullSafe)
        {
            var snippet = CodeSnippets.EqualityCheck(type, memberName, $"{otherName}.{memberName}", nullSafe);
            return snippet.Contains("||")
                ? $"({snippet})"
                : snippet;
        }

        private static IEqualsAttribute GetMemberAttribute(ISymbol symbol)
        {
            return GetFirstOrNull<IEqualsAttribute>(symbol)
                ?? new EqualsAttribute();
        }

        private static IAutoEqualsAttribute GetConfigAttribute(TypeInfo typeInfo)
        {
            return GetFirstOrNull<IAutoEqualsAttribute>(typeInfo.Symbol) 
                ?? new AutoEqualsAttribute();
        }

        public static T GetFirstOrNull<T>(ISymbol symbol) where T : class
        {
            var attData = symbol.AttributesOfType<T>().FirstOrDefault();
            if (attData is null)
                return null;
            return AttributeFactory.Create<T>(attData);
        }
    }
}
