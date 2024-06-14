using Antlr4.Runtime.Misc;
using Aspects.Parsers.CSharp.Generated;

namespace Aspects.Parsers.CSharp.Visitors
{
    internal class UsingVisitor : CSharpParserBaseVisitor<UsingDirective>
    {
        public override UsingDirective VisitUsing_directive([NotNull] CSharpParser.Using_directiveContext context)
        {
            context.
        }
    }
}
