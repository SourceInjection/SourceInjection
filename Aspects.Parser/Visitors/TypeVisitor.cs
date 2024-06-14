using Antlr4.Runtime.Misc;
using Aspects.Parsers.CSharp.Generated;

namespace Aspects.Parsers.CSharp.Visitors
{
    internal class TypeVisitor : CSharpParserBaseVisitor<TypeInfo>
    {
        public override TypeInfo VisitType_declaration([NotNull] CSharpParser.Type_declarationContext context)
        {
            return base.VisitType_declaration(context);
        }
    }
}
