using Aspects.SourceGenerators.Base;
using Microsoft.CodeAnalysis;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;
using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Common;
using Aspects.Attributes;
using System.Linq;

namespace Aspects
{
    [Generator]
    internal class ToStringSourceGenerator
        : ObjectMethodSourceGeneratorBase<IAutoToStringAttribute, IToStringAttribute, IToStringExcludeAttribute>
    {
        protected internal override string Name { get; } = nameof(ToString);

        protected override DataMemberPriority Priority { get; } = DataMemberPriority.Property;

        protected override IAutoToStringAttribute DefaultConfigAttribute => new AutoToStringAttribute();

        protected override IToStringAttribute DefaultMemberConfigAttribute => new ToStringAttribute();

        protected override string TypeBody(TypeInfo typeInfo)
        {
            var config = GetConfigAttribute(typeInfo);

            var sb = new StringBuilder();
            sb.AppendLine($"public override string {Name}()");
            sb.AppendLine("{");

            sb.Append(Code.Indent($"return $\"({typeInfo.Name})"));

            var symbols = GetSymbols(typeInfo, typeInfo.Members(true), config.DataMemberKind);

            if (symbols.Count > 0)
            {
                sb.Append("{{");
                sb.Append($"{symbols[0].Name}: {{{symbols[0].Name}}}");

                for(int i = 1; i < symbols.Count; i++)
                    sb.Append($", {symbols[i].Name}: {{{symbols[i].Name}}}");
                sb.Append("}}");
            }
            sb.AppendLine("\";");
            sb.Append('}');

            return sb.ToString();
        }
    }
}
