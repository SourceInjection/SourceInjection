using Aspects.Attributes;
using Aspects.SourceGenerators.Base;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators
{
    [Generator]
    public class HashCodeSourceGenerator : BasicMethodOverrideSourceGeneratorBase<HashCodeAttribute, HashCodeExcludeAttribute>
    {
        private protected override string Name => "HashCode";

        private protected override string ClassBody(TypeInfo typeInfo)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"public override int GetHashCode()");
            sb.AppendLine("{");

            sb.Append("\treturn HashCode.Combine(");

            var symbols = GetSymbols(typeInfo);
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
    }
}
