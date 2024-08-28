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

            sb.Append(CodeSnippets.Indent($"return $\"({typeInfo.NameWithGenericParameters})"));

            var names = GetMemberNames(typeInfo);

            if (names.Length > 0)
            {
                sb.Append("{{");
                sb.Append($"{names[0]}: {{{names[0]}}}");

                for(int i = 1; i < names.Length; i++)
                    sb.Append($", {names[i]}: {{{names[i]}}}");
                sb.Append("}}");
            }
            sb.AppendLine("\";");
            sb.Append('}');

            return sb.ToString();
        }

        private string[] GetMemberNames(TypeInfo typeInfo)
        {
            return GetPublicSymbols(typeInfo)
                .Where(sy => !IsExcluded(sy))
                .Select(sy => sy.Name)
                .Concat(typeInfo.Symbol.GetMembers()
                    .OfType<IFieldSymbol>()
                    .SelectMany(f => f.AttributesOfType<IGeneratesPublicPropertyFromFieldAttribute>()
                        .Select(a => GetAttribute(a)?.PropertyName(f))
                        .Where(s => !string.IsNullOrEmpty(s))))
                .Distinct()
                .ToArray();
        }

        private IGeneratesPublicPropertyFromFieldAttribute GetAttribute(AttributeData attributeData)
        {
            if (!AttributeFactory.TryCreate<IGeneratesPublicPropertyFromFieldAttribute>(attributeData, out var result))
                return null;
            return result;
        }

        private static bool IsExcluded(ISymbol symbol)
        {
            return !symbol.HasAttributeOfType<IToStringAttribute>() && (
                symbol is IPropertySymbol p && p.Type.IsEnumerable()
                || symbol is IFieldSymbol f && (f.Type.IsEnumerable() || f.HasAttributeOfType<IGeneratesPublicPropertyFromFieldAttribute>()));
        }
    }
}
