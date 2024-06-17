using static Aspects.Parsers.CSharp.Generated.CSharpParser;

namespace Aspects.Parsers.CSharp.Visitors.Common
{
    internal static class ExternAliases
    {
        public static List<ExternAliasDefinition> FromContext(Extern_alias_directivesContext context)
        {
            return context.extern_alias_directive()
                .Select(c => new ExternAliasDefinition(c.identifier().GetText())).ToList();
        }
    }
}
