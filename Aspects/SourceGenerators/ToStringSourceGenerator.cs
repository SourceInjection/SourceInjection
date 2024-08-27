using Aspects.SourceGenerators.Base;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;
using Aspects.Attributes.Interfaces;
using Aspects.Util;
using Aspects.SourceGenerators.Common;
using Aspects.Attributes;

namespace Aspects.SourceGenerators
{
    [Generator]
    internal class ToStringSourceGenerator 
        : ObjectMethodSourceGeneratorBase<IAutoToStringAttribute, IToStringAttribute, IToStringExcludeAttribute>
    {
        protected internal override string Name { get; } = nameof(ToString);

        protected override DataMemberPriority Priority { get; } = DataMemberPriority.Property;

        protected override DataMemberKind DataMemberKindFromAttribute(IAutoToStringAttribute attr)
        {
            return attr.DataMemberKind;
        }

        protected override string TypeBody(TypeInfo typeInfo)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"public override string {Name}()");
            sb.AppendLine("{");

            sb.Append(CodeSnippets.Indent($"return $\"({typeInfo.Name})"));

            var symbols = GetPublicSymbols(typeInfo)
                .Where(sy => !IsExcluded(sy));

            if (symbols.Any())
            {
                sb.Append("{{");
                var first = symbols.First().Name;
                sb.Append($"{first}: {{{first}}}");

                foreach (var name in symbols.Skip(1).Select(s => s.Name))
                    sb.Append($", {name}: {{{name}}}");
                sb.Append("}}");
            }
            sb.AppendLine("\";");
            sb.Append('}');

            return sb.ToString();
        }

        private static bool IsExcluded(ISymbol symbol)
        {
            return !symbol.HasAttributeOfType<IToStringAttribute>() && (
                symbol is IPropertySymbol p && p.Type.IsEnumerable()
                || symbol is IFieldSymbol f && f.Type.IsEnumerable());
        }
    }
}
