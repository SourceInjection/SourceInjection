using static Aspects.Parsers.CSharp.Generated.CSharpParser;

namespace Aspects.Parsers.CSharp.Visitors.Common
{
    internal static class Types
    {
        public static List<TypeDefinition> FromContext(Namespace_member_declarationsContext context)
        {
            var types = new List<TypeDefinition>();
            var visitor = new TypeVisitor();

            var typeContexts = context.namespace_member_declaration()
                .Where(c => c.type_declaration() is not null)
                .Select(c => c.type_declaration());

            foreach (var c in typeContexts)
            {
                var type = visitor.VisitType_declaration(c);
                if (type is not null)
                    types.Add(type);
            }
            return types;
        }
    }
}
