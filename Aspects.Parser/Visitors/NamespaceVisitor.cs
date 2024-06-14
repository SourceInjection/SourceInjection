using Antlr4.Runtime.Misc;
using Aspects.Parsers.CSharp.Generated;
using static Aspects.Parsers.CSharp.Generated.CSharpParser;

namespace Aspects.Parsers.CSharp.Visitors
{
    internal class NamespaceVisitor : CSharpParserBaseVisitor<NamespaceInfo>
    {
        public override NamespaceInfo VisitCompilation_unit([NotNull] Compilation_unitContext context)
        {
            return new NamespaceInfo(
                name:          string.Empty,
                directives:    GetUsingDirectivesFromContext(context.using_directives()),
                namespaces:    GetNamespacesFromContext(context.namespace_member_declarations()),
                types:         GetTypesFromContext(context.namespace_member_declarations()),
                externAliases: GetExternAliasFromContext(context.extern_alias_directives()));
        }

        public override NamespaceInfo VisitNamespace_declaration([NotNull] Namespace_declarationContext context)
        {
            var name = context.qualified_identifier().GetText();
            return GetNamespaceFromBody(name, context.namespace_body());
        }

        private NamespaceInfo GetNamespaceFromBody(string name, Namespace_bodyContext context)
        {
            return new NamespaceInfo(
                name:          name,
                directives:    GetUsingDirectivesFromContext(context.using_directives()),
                namespaces:    GetNamespacesFromContext(context.namespace_member_declarations()),
                types:         GetTypesFromContext(context.namespace_member_declarations()),
                externAliases: GetExternAliasFromContext(context.extern_alias_directives()));
        }

        private static List<ExternAliasInfo> GetExternAliasFromContext(Extern_alias_directivesContext context)
        {
            return context.extern_alias_directive()
                .Select(c => new ExternAliasInfo(c.identifier().GetText())).ToList();
        }

        private static List<UsingDirectiveInfo> GetUsingDirectivesFromContext(Using_directivesContext context)
        {
            var usingDirectives = new List<UsingDirectiveInfo>();
            var visitor = new UsingVisitor();
            foreach (var c in context.using_directive())
            {
                var directive = visitor.VisitUsing_directive(c);
                if(directive is not null)
                    usingDirectives.Add(directive);
            }
            return usingDirectives;
        }


        private static List<TypeInfo> GetTypesFromContext(Namespace_member_declarationsContext context)
        {
            var types = new List<TypeInfo>();
            var visitor = new TypeVisitor();

            var typeContexts = context.namespace_member_declaration()
                .Where(c => c.type_declaration() is not null)
                .Select(c => c.type_declaration());

            foreach (var c in typeContexts)
            {
                var type = visitor.VisitType_declaration(c);
                if(type is not null)
                    types.Add(type);
            }
            return types;
        }

        private List<NamespaceInfo> GetNamespacesFromContext(Namespace_member_declarationsContext context)
        {
            var namespaces = new List<NamespaceInfo>();

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
