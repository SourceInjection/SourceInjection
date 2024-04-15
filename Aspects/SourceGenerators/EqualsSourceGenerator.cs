using Aspects.Attributes;
using Aspects.SourceGenerators.Base;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators
{
    [Generator]
    internal class EqualsSourceGenerator : BasicMethodOverrideSourceGeneratorBase<EqualsAttribute, EqualsExcludeAttribute>
    {
        private const string arg = "obj";
        private const string other = "other";

        private protected override string Name => "Equals";


        private protected override string ClassBody(TypeInfo typeInfo)
        {
            var sb = new StringBuilder();
            sb.AppendLine("#nullable enable");
            sb.AppendLine($"public override bool Equals(object? {arg})");
            sb.AppendLine("{");

            sb.Append($"\treturn {arg} is {typeInfo.Name}");

            var symbols = GetSymbols(typeInfo);
            if (symbols.Any())
            {
                sb.Append($" {other}");
                foreach (var symbol in symbols)
                {
                    sb.AppendLine();
                    sb.Append($"\t\t&& {MemberEquals(symbol)}");
                }
            }
            sb.AppendLine(";");

            sb.AppendLine("}");
            sb.Append("#nullable restore");
            return sb.ToString();
        }

        private static string MemberEquals(ISymbol symbol)
        {
            var memberName = symbol.Name;

            if (symbol is IFieldSymbol field)
            {
                if (field.Type.IsReferenceType)
                    return $"{memberName}?.Equals({other}.{memberName}) is true";
                return $"{memberName}.Equals({other}.{memberName})";
            }
            else if (symbol is IPropertySymbol property)
            {
                if (property.Type.IsReferenceType)
                    return $"{memberName}?.Equals({other}.{memberName}) is true";
                return $"{memberName}.Equals({other}.{memberName})";
            }
            else throw new NotImplementedException();
        }
    }
}
