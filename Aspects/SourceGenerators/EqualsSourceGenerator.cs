using Aspects.Attributes;
using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Base;
using Aspects.SourceGenerators.Common;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;
using System.Text;
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

            var symbols = GetLocalTargetedSymbols(typeInfo);
            if (symbols.Any())
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
                    GetNullSafety(symbol, config));

                sb.AppendLine().Append(CodeSnippets.Indent($"&& {memberEquals}", 2));
            }

            sb.AppendLine(";");

            AppendMethodEnd(typeInfo, sb);
            return sb.ToString();
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

        private static NullSafety GetNullSafety(ISymbol symbol, IAutoEqualsAttribute config)
        {
            var memberAttribute = GetMemberAttribute(symbol);

            if (memberAttribute.NullSafety != NullSafety.Auto)
                return memberAttribute.NullSafety;
            if (symbol.HasNotNullAttribute())
                return NullSafety.Off;
            if(symbol.HasMaybeNullAttribute())
                return NullSafety.On;
            return config.NullSafety;
        }

        private static string MemberEquals(ISymbol symbol, bool nullableEnabled, NullSafety nullSafety)
        {
            var type = GetType(symbol);
            var nullSafe = nullSafety == NullSafety.On ||
                nullSafety == NullSafety.Auto && (!nullableEnabled || type.HasNullableAnnotation());

            return Comparison(type, symbol.Name, nullSafe);
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
            return GetAttribute<IEqualsAttribute>(symbol, new EqualsAttribute());
        }

        private static IAutoEqualsAttribute GetConfigAttribute(TypeInfo typeInfo)
        {
            return GetAttribute<IAutoEqualsAttribute>(typeInfo.Symbol, new AutoEqualsAttribute());
        }

        private static T GetAttribute<T>(ISymbol symbol, T defaultValue)
        {
            var attData = symbol.AttributesOfType<T>().FirstOrDefault();
            if (attData is null || !AttributeFactory.TryCreate<T>(attData, out var config))
                return defaultValue;
            return config;
        }
    }
}
