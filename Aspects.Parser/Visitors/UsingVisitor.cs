using Antlr4.Runtime.Misc;
using Aspects.Parsers.CSharp.Generated;

namespace Aspects.Parsers.CSharp.Visitors
{
    internal class UsingVisitor : CSharpParserBaseVisitor<UsingDirectiveDefinition>
    {
        public override UsingDirectiveDefinition VisitUsingAliasDirective([NotNull] CSharpParser.UsingAliasDirectiveContext context)
        {
            return new UsingAliasDirectiveDefinition(context.GetText(), context.identifier().GetText(), context.namespace_or_type_name().GetText());
        }

        public override UsingDirectiveDefinition VisitUsingNamespaceDirective([NotNull] CSharpParser.UsingNamespaceDirectiveContext context)
        {
            return new UsingNamespaceDirectiveDefinition(context.GetText(), context.namespace_or_type_name().GetText());
        }

        public override UsingDirectiveDefinition VisitUsingStaticDirective([NotNull] CSharpParser.UsingStaticDirectiveContext context)
        {
            return new UsingStaticDirectiveDefinition(context.GetText(), context.namespace_or_type_name().GetText());
        }
    }
}
