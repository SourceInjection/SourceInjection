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

        protected override string Name { get; } = nameof(Equals);

        protected override DataMemberPriority Priority { get; } = DataMemberPriority.Field;

        protected override string ClassBody(TypeInfo typeInfo)
        {
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

            if (ShouldIncludeBase(typeInfo))
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

            sb.AppendLine("}");
            if(typeInfo.HasNullableEnabled)
                sb.Append("#nullable restore");
            return sb.ToString();
        }

        private bool ShouldIncludeBase(TypeInfo typeInfo)
        {
            return typeInfo.Symbol.BaseType is ITypeSymbol syBase && (
                syBase.HasAttributeOfType<IEqualsConfigAttribute>()
                || syBase.OverridesEquals()
                || syBase.Inheritance().Any(
                    sy => sy.HasAttributeOfType<IEqualsConfigAttribute>()
                    || sy.GetMembers().Any(m => m.HasAttributeOfType<IEqualsAttribute>())));
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
    }
}
