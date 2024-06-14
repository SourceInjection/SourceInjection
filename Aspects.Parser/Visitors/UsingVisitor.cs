using Antlr4.Runtime.Misc;
using Aspects.Parsers.CSharp.Generated;

namespace Aspects.Parsers.CSharp.Visitors
{
    internal class UsingVisitor : CSharpParserBaseVisitor<UsingDirectiveInfo>
    {
        public override UsingDirectiveInfo VisitUsingAliasDirective([NotNull] CSharpParser.UsingAliasDirectiveContext context)
        {
            return new UsingAliasDirectiveInfo(context.GetText(), context.identifier().GetText(), context.namespace_or_type_name().GetText());
        }

        public override UsingDirectiveInfo VisitUsingNamespaceDirective([NotNull] CSharpParser.UsingNamespaceDirectiveContext context)
        {
            return new UsingNamespaceDirectiveInfo(context.GetText(), context.namespace_or_type_name().GetText());
        }

        public override UsingDirectiveInfo VisitUsingStaticDirective([NotNull] CSharpParser.UsingStaticDirectiveContext context)
        {
            return new UsingStaticDirectiveInfo(context.GetText(), context.namespace_or_type_name().GetText());
        }

        public override UsingDirectiveInfo VisitUsingTupleTypeDefinition([NotNull] CSharpParser.UsingTupleTypeDefinitionContext context)
        {
            return new UsingTupleTypeDefinitionInfo(context.GetText(), (TupleInfo)new TypeVisitor().VisitTuple_type(context.tuple_type()));
        }
    }
}
