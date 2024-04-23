using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Base;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators
{
    [Generator]
    internal class EqualsSourceGenerator : BasicMethodOverrideSourceGeneratorBase<IEqualsAttribute, IEqualsExcludeAttribute>
    {
        private const string arg = "obj";
        private const string other = "other";

        private protected override string Name => "Equals";

        private protected override string ClassBody(TypeInfo typeInfo)
        {
            var sb = new StringBuilder();
            sb.AppendLine("#nullable enable");
            sb.AppendLine($"public override bool {Name}(object? {arg})");
            sb.AppendLine("{");

            sb.Append($"\treturn");
            if (typeInfo.Symbol.IsReferenceType)
                sb.Append($" {arg} == this ||");

            sb.Append($" {arg} is {typeInfo.Name}");

            var symbols = GetDeclaredSymbols(typeInfo);
            if (symbols.Any())
                sb.Append($" {other}");

            if (ShouldIncludeBase(typeInfo))
            {
                sb.AppendLine();
                sb.Append($"\t\t&& base.{Name}({arg})");
            }

            foreach (var symbol in symbols)
            {
                sb.AppendLine();
                sb.Append($"\t\t&& {MemberEquals(symbol)}");
            }
            
            sb.AppendLine(";");

            sb.AppendLine("}");
            sb.Append("#nullable restore");
            return sb.ToString();
        }

        private bool ShouldIncludeBase(TypeInfo typeInfo)
        {
            return typeInfo.Symbol.HasAttributeOfType<IEqualsAttribute>()
                || typeInfo.Symbol.OverridesEquals()
                || typeInfo.Inheritance().SelectMany(cl => cl.GetMembers()).Any(m => m.HasAttributeOfType<IEqualsAttribute>());
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
                {
                    var method = $"{typeof(Enumerable).FullName}.{nameof(Enumerable.SequenceEquals)}";
                    return $"{method}({memberName}, {other}.{memberName})";
                }
                return $"{memberName}?.{Name}({other}.{memberName}) is true";
            }
            return $"{memberName}.{Name}({other}.{memberName})";
        }
    }
}
