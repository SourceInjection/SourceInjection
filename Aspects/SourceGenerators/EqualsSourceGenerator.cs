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
        : ObjectMethodSourceGeneratorBase<IEqualsConfigAttribute, IEqualsAttribute, IEqualsExcludeAttribute>
    {
        private const string argName = "obj";
        private const string otherName = "other";

        protected internal override string Name { get; } = nameof(Equals);

        protected override DataMemberPriority Priority { get; } = DataMemberPriority.Field;

        protected override DataMemberKind DataMemberKindFromAttribute(IEqualsConfigAttribute attr)
        {
            return attr.DataMemberKind;
        }

        protected override string ClassBody(TypeInfo typeInfo)
        {
            var config = GetConfigAttribute(typeInfo);

            var sb = new StringBuilder();

            if(typeInfo.HasNullableEnabled)
                sb.AppendLine("#nullable enable");
            sb.Append($"public override bool {Name}(object");
            if (typeInfo.HasNullableEnabled)
                sb.Append('?');
            sb.AppendLine($" {argName})");
            sb.AppendLine("{");

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
                sb.AppendLine();
                sb.Append(CodeSnippets.Indent($"&& {MemberEquals(symbol, typeInfo.HasNullableEnabled)}", 2));
            }
            
            sb.AppendLine(";");

            sb.Append("}");
            if (typeInfo.HasNullableEnabled)
            {
                sb.AppendLine();
                sb.Append("#nullable restore");
            }
            return sb.ToString();
        }

        private bool ShouldIncludeBase(TypeInfo typeInfo, IEqualsConfigAttribute configAttribute)
        {
            return configAttribute.ForceIncludeBase || typeInfo.Symbol.IsReferenceType 
                && typeInfo.Symbol.BaseType is ITypeSymbol syBase 
                && (syBase.HasAttributeOfType<IEqualsConfigAttribute>() || syBase.OverridesEquals());
        }

        private string MemberEquals(ISymbol symbol, bool nullableEnabled)
        {
            var type = GetType(symbol);
            var nullSafe = !nullableEnabled || type.HasNullableAnnotation();
            return Comparison(type, symbol.Name, nullSafe);
        }

        private ITypeSymbol GetType(ISymbol symbol)
        {
            if (symbol is IFieldSymbol field)
                return field.Type;
            if (symbol is IPropertySymbol property)
                return property.Type;
            throw new NotImplementedException();
        }

        private string Comparison(ITypeSymbol type, string memberName, bool nullSafe)
        {
            var snippet = CodeSnippets.EqualityCheck(type, memberName, $"{otherName}.{memberName}", nullSafe);
            return snippet.Contains("||")
                ? $"({snippet})"
                : snippet;
        }

        private static IEqualsConfigAttribute GetConfigAttribute(TypeInfo typeInfo)
        {
            var attData = typeInfo.Symbol.AttributesOfType<IEqualsConfigAttribute>().FirstOrDefault();
            if (attData is null || !AttributeFactory.TryCreate<IEqualsConfigAttribute>(attData, out var config))
                return new AutoEqualsAttribute();
            return config;
        }
    }
}
