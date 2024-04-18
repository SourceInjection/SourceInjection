using Aspects.Attributes;
using Aspects.SourceGenerators.Base;
using Aspects.SourceGenerators.Common;
using Aspects.SyntaxReceivers;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators
{
    [Generator]
    public class HashCodeSourceGenerator : BasicMethodOverrideSourceGeneratorBase
    {
        private protected override string Name => "HashCode";

        protected override ISet<string> TypeAttributes { get; }
            = new HashSet<string>() { typeof(EqualsAndHashCodeAttribute).FullName, typeof(HashCodeAttribute).FullName };

        protected override ISet<string> ExcludeAttributes { get; }
            = new HashSet<string>() { typeof(EqualsAndHashCodeExcludeAttribute).FullName, typeof(HashCodeExcludeAttribute).FullName };

        private protected override TypeSyntaxReceiver SyntaxReceiver { get; } = new TypeSyntaxReceiver(
                Types.With<HashCodeAttribute>()
            .Or(Types.With<EqualsAndHashCodeAttribute>())
            .Or(Types.WithMembersWith<HashCodeAttribute>())
            .Or(Types.WithMembersWith<EqualsAndHashCodeAttribute>()));

        private protected override string ClassBody(TypeInfo typeInfo)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"public override int GetHashCode()");
            sb.AppendLine("{");

            sb.Append("\treturn HashCode.Combine(");

            var symbols = GetDeclaredSymbols(typeInfo);

            if (ShouldIncludeBase(typeInfo))
            {
                sb.AppendLine();
                sb.Append($"\t\tbase.GetHashCode()");
                if(symbols.Any())
                    sb.Append(',');
            }

            if (symbols.Any())
            {
                sb.AppendLine();
                sb.Append($"\t\t{symbols.First().Name}");
                foreach (var symbol in symbols.Skip(1))
                {
                    sb.AppendLine(",");
                    sb.Append($"\t\t{symbol.Name}");
                }
            }
            sb.AppendLine(");");

            sb.Append('}');
            return sb.ToString();
        }

        private bool ShouldIncludeBase(TypeInfo typeInfo)
        {
            var inheritance = typeInfo.Inheritance();
            if (inheritance.Any(sy => sy.HasAnyAttribute(TypeAttributes)))
                return true;

            var baseMembers = inheritance.SelectMany(cl => cl.GetMembers());
            return baseMembers.Any(m => m is IMethodSymbol method && MethodIsHashCodeOverride(method))
                || baseMembers.Any(m => m.HasAnyAttribute(TypeAttributes));
        }

        private bool MethodIsHashCodeOverride(IMethodSymbol method)
        {
            return method.Name == "GetHashCode"
                && method.IsOverride
                && method.Parameters.Length == 0;
        }
    }
}
