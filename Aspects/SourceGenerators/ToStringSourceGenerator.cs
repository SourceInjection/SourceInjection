using Aspects.Attributes;
using Aspects.SourceGenerators.Base;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;
using Accessibility = Microsoft.CodeAnalysis.Accessibility;
using System.Collections.Generic;
using Aspects.SourceGenerators.Common;
using Aspects.SyntaxReceivers;
using Aspects.Util;

namespace Aspects.SourceGenerators
{
    [Generator]
    internal class ToStringSourceGenerator : BasicMethodOverrideSourceGeneratorBase
    {
        private protected override string Name => "ToString";

        protected override ISet<string> TypeAttributes { get; } 
            = new HashSet<string>() { typeof(ToStringAttribute).FullName };

        protected override ISet<string> ExcludeAttributes { get; } 
            = new HashSet<string>() { typeof(ToStringExcludeAttribute).FullName };

        private protected override TypeSyntaxReceiver SyntaxReceiver { get; } = new TypeSyntaxReceiver(
                Types.With<ToStringAttribute>()
            .Or(Types.WithMembersWith<ToStringAttribute>()));

        private protected override string ClassBody(TypeInfo typeInfo)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"public override string ToString()");
            sb.AppendLine("{");

            sb.Append($"\treturn $\"({typeInfo.Name})");

            var symbols = GetAllSymbols(typeInfo)
                .Where(sy => IsPublicProperty(sy) || IsPublicField(sy));

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

        public static bool IsPublicProperty(ISymbol symbol)
        {
            return symbol is IPropertySymbol property
                && property.DeclaredAccessibility == Accessibility.Public
                && property.GetMethod != null
                && (property.GetMethod.DeclaredAccessibility == Accessibility.NotApplicable 
                    || property.GetMethod.DeclaredAccessibility == Accessibility.Public);
        }

        public static bool IsPublicField(ISymbol symbol)
        {
            return symbol is IFieldSymbol field
                && field.DeclaredAccessibility == Accessibility.Public;
        }
    }
}
