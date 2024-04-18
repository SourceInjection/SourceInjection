using Aspects.Attributes;
using Aspects.SourceGenerators.Base;
using Aspects.SourceGenerators.Common;
using Aspects.SyntaxReceivers;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators
{
    [Generator]
    internal class EqualsSourceGenerator : BasicMethodOverrideSourceGeneratorBase
    {
        private const string arg = "obj";
        private const string other = "other";

        private protected override string Name => "Equals";

        protected override ISet<string> TypeAttributes { get; } 
            = new HashSet<string>() { typeof(EqualsAndHashCodeAttribute).FullName, typeof(EqualsAttribute).FullName };

        protected override ISet<string> ExcludeAttributes { get; }
            = new HashSet<string>() { typeof(EqualsAndHashCodeExcludeAttribute).FullName, typeof(EqualsExcludeAttribute).FullName };

        private protected override TypeSyntaxReceiver SyntaxReceiver { get; } = new TypeSyntaxReceiver(
                Types.With<EqualsAttribute>()
            .Or(Types.With<EqualsAndHashCodeAttribute>())
            .Or(Types.WithMembersWith<EqualsAttribute>())
            .Or(Types.WithMembersWith<EqualsAndHashCodeAttribute>()));

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
            var inheritance = typeInfo.Inheritance();
            if (inheritance.Any(sy => sy.HasAnyAttribute(TypeAttributes)))
                return true;

            var baseMembers = inheritance.SelectMany(cl => cl.GetMembers());
            return baseMembers.Any(m => m is IMethodSymbol method && MethodIsEqualsOverride(method))
                || baseMembers.Any(m => m.HasAnyAttribute(TypeAttributes));
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
