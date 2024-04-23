using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Base;
using Microsoft.CodeAnalysis;
using System;
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
            sb.AppendLine($"public override bool Equals(object? {arg})");
            sb.AppendLine("{");

            sb.Append($"\treturn {arg} is {typeInfo.Name}");

            var symbols = GetDeclaredSymbols(typeInfo);
            if (symbols.Any())
                sb.Append($" {other}");
            
            if(ShouldIncludeBase(typeInfo))
                sb.Append($" && base.Equals({arg})");

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
            return typeInfo.Inheritance()
                .SelectMany(cl => cl.GetMembers())
                .Any(m => m is IMethodSymbol method && MethodIsEqualsOverride(method));
        }

        private bool MethodIsEqualsOverride(IMethodSymbol method)
        {
            return method.Name == "Equals"
                && method.IsOverride
                && method.Parameters.Length == 1
                && (method.Parameters[0].Type.ToDisplayString() == "object" || method.Parameters[0].Type.ToDisplayString() == "object?");
        }

        private static string MemberEquals(ISymbol symbol)
        {
            var memberName = symbol.Name;

            if (symbol is IFieldSymbol field)
                return Comparison(field.Type, memberName);
            else if (symbol is IPropertySymbol property)
                return Comparison(property.Type, memberName);
            else throw new NotImplementedException();
        }

        protected static string Comparison(ITypeSymbol type, string memberName)
        {
            if (type.IsReferenceType)
                return $"{memberName}?.Equals({other}.{memberName}) is true";
            return $"{memberName}.Equals({other}.{memberName})";
        }
    }
}
