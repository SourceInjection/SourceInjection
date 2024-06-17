using Antlr4.Runtime.Misc;
using Aspects.Parsers.CSharp.Generated;
using Aspects.Parsers.CSharp.Visitors.Common;
using static Aspects.Parsers.CSharp.Generated.CSharpParser;

namespace Aspects.Parsers.CSharp.Visitors
{
    internal class NamespaceVisitor : CSharpParserBaseVisitor<NamespaceDefinition>
    {
        public override NamespaceDefinition VisitCompilation_unit([NotNull] Compilation_unitContext context)
        {
            return new NamespaceDefinition(
                name:          string.Empty,
                directives:    UsingDirectives.FromContext(context.using_directives()),
                namespaces:    GetNamespacesFromContext(context.namespace_member_declarations()),
                types:         Types.FromContext(context.namespace_member_declarations()),
                externAliases: ExternAliases.FromContext(context.extern_alias_directives()));
        }

        public override NamespaceDefinition VisitNamespace_declaration([NotNull] Namespace_declarationContext context)
        {
            var name = context.qualified_identifier().GetText();
            return GetNamespaceFromBody(name, context.namespace_body());
        }

        private NamespaceDefinition GetNamespaceFromBody(string name, Namespace_bodyContext context)
        {
            return new NamespaceDefinition(
                name:          name,
                directives:    UsingDirectives.FromContext(context.using_directives()),
                namespaces:    GetNamespacesFromContext(context.namespace_member_declarations()),
                types:         Types.FromContext(context.namespace_member_declarations()),
                externAliases: ExternAliases.FromContext(context.extern_alias_directives()));
        }

        private List<NamespaceDefinition> GetNamespacesFromContext(Namespace_member_declarationsContext context)
        {
            var namespaces = new List<NamespaceDefinition>();

            var nsContexts = context.namespace_member_declaration()
                .Where(c => c.namespace_declaration() is not null)
                .Select(c => c.namespace_declaration());

            foreach (var c in nsContexts)
            {
                var ns = VisitNamespace_declaration(c);
                if(ns is not null)
                    namespaces.Add(ns);
            }
            return namespaces;
        }
    }
}
