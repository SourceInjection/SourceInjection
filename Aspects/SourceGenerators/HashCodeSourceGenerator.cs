using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Base;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators
{
    [Generator]
    public class HashCodeSourceGenerator : BasicMethodOverrideSourceGeneratorBase<IHashCodeAttribute, IHashCodeExcludeAttribute>
    {
        private protected override string Name => "GetHashCode";

        private protected override string ClassBody(TypeInfo typeInfo)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"public override int {Name}()");
            sb.AppendLine("{");

            sb.Append("\treturn HashCode.Combine(");

            var symbols = GetDeclaredSymbols(typeInfo);

            if (ShouldIncludeBase(typeInfo))
            {
                sb.AppendLine();
                sb.Append($"\t\tbase.{Name}()");
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
            return typeInfo.Inheritance()
                .SelectMany(cl => cl.GetMembers())
                .Any(m => m is IMethodSymbol method && MethodIsHashCodeOverride(method));
        }

        private bool MethodIsHashCodeOverride(IMethodSymbol method)
        {
            return method.Name == Name
                && method.IsOverride
                && method.Parameters.Length == 0;
        }
    }
}
