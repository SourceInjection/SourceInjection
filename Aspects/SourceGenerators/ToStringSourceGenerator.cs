using Aspects.SourceGenerators.Base;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;
using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Common;
using Aspects.Util;

namespace Aspects.SourceGenerators
{
    [Generator]
    internal class ToStringSourceGenerator 
        : BasicMethodOverrideSourceGeneratorBase<IToStringConfigAttribute, IToStringAttribute, IToStringExcludeAttribute>
    {
        protected override string Name { get; } = nameof(ToString);

        protected override DataMemberPriority Priority { get; } = DataMemberPriority.Property;

        protected override string ClassBody(TypeInfo typeInfo)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"public override string {Name}()");
            sb.AppendLine("{");

            sb.Append($"\treturn $\"({typeInfo.Name})");

            var symbols = GetPublicSymbols(typeInfo);
            if (!CollectionsEnabled(typeInfo))
            {
                symbols = symbols.Where(sy =>
                    sy is IPropertySymbol p && !p.Type.IsEnumerable()
                    || sy is IFieldSymbol f && !f.Type.IsEnumerable());
            }

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

        private bool CollectionsEnabled(TypeInfo typeInfo)
        {
            return AttributeFactory.TryCreate<IToStringConfigAttribute>(
                    typeInfo.Symbol.AttributesOfType<IToStringConfigAttribute>().FirstOrDefault(), out var att) 
                && att.CollectionsEnabled;
        }
    }
}
