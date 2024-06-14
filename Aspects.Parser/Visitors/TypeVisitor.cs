using Antlr4.Runtime.Misc;
using Aspects.Parsers.CSharp.Generated;
using static Aspects.Parsers.CSharp.Generated.CSharpParser;

namespace Aspects.Parsers.CSharp.Visitors
{
    internal class TypeVisitor : CSharpParserBaseVisitor<TypeInfo>
    {
        public override TypeInfo VisitTuple_type([NotNull] Tuple_typeContext context)
        {
            return new TupleInfo(context.tuple_element()
                .Select(c => new TupleMemberInfo(c.type_().GetText(), c.identifier()?.GetText()))
                .ToArray());
        }


        public override TypeInfo VisitType_declaration([NotNull] Type_declarationContext context)
        {
            return base.VisitType_declaration(context);
        }
    }
}
