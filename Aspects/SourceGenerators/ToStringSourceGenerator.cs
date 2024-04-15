﻿using Aspects.Attributes;
using Aspects.SourceGenerators.Base;
using Aspects.SourceGenerators.Common;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators
{
    [Generator]
    internal class ToStringSourceGenerator : BasicMethodOverrideSourceGeneratorBase<ToStringAttribute, ToStringExcludeAttribute>
    {
        private protected override string Name => "ToString";

        private protected override string ClassBody(TypeInfo typeInfo)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"public override string ToString()");
            sb.AppendLine("{");

            sb.Append($"\treturn $\"({typeInfo.Name})");

            var symbols = GetSymbols(typeInfo);
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
    }
}
