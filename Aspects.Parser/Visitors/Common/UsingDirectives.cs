using static Aspects.Parsers.CSharp.Generated.CSharpParser;

namespace Aspects.Parsers.CSharp.Visitors.Common
{
    internal class UsingDirectives
    {
        public static List<UsingDirectiveDefinition> FromContext(Using_directivesContext context)
        {
            var usingDirectives = new List<UsingDirectiveDefinition>();
            var visitor = new UsingVisitor();
            foreach (var c in context.using_directive())
            {
                var directive = visitor.VisitUsing_directive(c);
                if (directive is not null)
                    usingDirectives.Add(directive);
            }
            return usingDirectives;
        }
    }
}
