using Aspects.Interfaces;
using Aspects.SourceGeneration.Base;
using Microsoft.CodeAnalysis;
using System.Text;
using TypeInfo = Aspects.SourceGeneration.Common.TypeInfo;
using Aspects.SourceGeneration;
using System.Linq;

#pragma warning disable IDE0130

namespace Aspects
{
    [Generator]
    internal class SGToString
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

            sb.Append(Snippets.Indent($"return $\"({typeInfo.Name})"));

            var symbols = GetSymbols(typeInfo, typeInfo.Members(true), config.DataMemberKind)
                .Where(m => m.DeclaredAccessibility == Microsoft.CodeAnalysis.Accessibility.Public).ToArray();

            if (symbols.Length > 0)
            {
                sb.Append("{{");
                sb.Append($"{symbols[0].Name}: {{{symbols[0].Name}}}");

                for (int i = 1; i < symbols.Length; i++)
                    sb.Append($", {symbols[i].Name}: {{{symbols[i].Name}}}");
                sb.Append("}}");
            }
            sb.AppendLine("\";");
            sb.Append('}');

            return sb.ToString();
        }
    }
}

#pragma warning restore IDE0130