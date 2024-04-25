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
    public sealed class EqualsSourceGenerator 
        : BasicMethodOverrideSourceGeneratorBase<IEqualsConfigAttribute, IEqualsAttribute, IEqualsExcludeAttribute>
    {
        private const string argName = "obj";
        private const string otherName = "other";

        private protected override string Name { get; } = nameof(Equals);

        private protected override string ClassBody(TypeInfo typeInfo)
        {
            var sb = new StringBuilder();

            var useNullable = CanUseNullable(typeInfo);

            if(useNullable)
                sb.AppendLine("#nullable enable");
            sb.Append($"public override bool {Name}(object");
            if (useNullable)
                sb.Append('?');
            sb.AppendLine($" {argName})");
            sb.AppendLine("{");

            sb.Append($"\treturn");
            if (typeInfo.Symbol.IsReferenceType)
                sb.Append($" {argName} == this ||");

            sb.Append($" {argName} is {typeInfo.Name}");

            var symbols = GetRelevantSymbols(typeInfo);
            if (symbols.Any())
                sb.Append($" {otherName}");

            if (ShouldIncludeBase(typeInfo))
            {
                sb.AppendLine();
                sb.Append($"\t\t&& base.{Name}({argName})");
            }

            foreach (var symbol in symbols)
            {
                sb.AppendLine();
                sb.Append($"\t\t&& {MemberEquals(symbol)}");
            }
            
            sb.AppendLine(";");

            sb.AppendLine("}");
            if(useNullable)
                sb.Append("#nullable restore");
            return sb.ToString();
        }

        private bool CanUseNullable(TypeInfo typeInfo)
        {
            return typeInfo.Symbol.Inheritance()
                .SelectMany(cl => cl.GetMembers())
                .OfType<IMethodSymbol>()
                .Any(m => m.Name == nameof(Equals) 
                    && !m.IsOverride 
                    && m.IsVirtual 
                    && m.Parameters.Any(p => p.NullableAnnotation == NullableAnnotation.Annotated));
        }

        private bool ShouldIncludeBase(TypeInfo typeInfo)
        {
            return typeInfo.Symbol.HasAttributeOfType<IEqualsConfigAttribute>()
                || typeInfo.Symbol.OverridesEquals()
                || typeInfo.Symbol.Inheritance().SelectMany(cl => cl.GetMembers())
                    .Any(m => m.HasAttributeOfType<IEqualsAttribute>());
        }

        private string MemberEquals(ISymbol symbol)
        {
            var memberName = symbol.Name;

            if (symbol is IFieldSymbol field)
                return Comparison(field.Type, memberName);
            else if (symbol is IPropertySymbol property)
                return Comparison(property.Type, memberName);
            else throw new NotImplementedException();
        }

        private string Comparison(ITypeSymbol type, string memberName)
        {
            if (type.IsReferenceType)
            {
                if (type.IsEnumerable() && !type.OverridesEquals())
                    return Output.SequenceEqualsMethod(memberName, $"{otherName}.{memberName}");

                return $"({memberName} is null && {otherName}.{memberName} is null " +
                    $"|| {memberName}?.{Name}({otherName}.{memberName}) is true)";
            }
            return $"{memberName}.{Name}({otherName}.{memberName})";
        }
    }
}
