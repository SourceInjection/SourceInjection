using static Aspects.Parsers.CSharp.Generated.CSharpParser;

namespace Aspects.Parsers.CSharp.Visitors.Common
{
    internal static class Members
    {
        public static List<MemberDefinition> FromContext(Struct_bodyContext? context)
        {
            var members = new List<MemberDefinition>();
            if (context is null)
                return members;

            var visitor = new MemberVisitor();
            foreach (var c in context.struct_member_declaration())
            {
                var member = visitor.VisitStruct_member_declaration(c);
                if (member is not null)
                    members.Add(member);
            }
            return members;
        }

        public static List<MemberDefinition> FromContext(Class_bodyContext? context)
        {
            var members = new List<MemberDefinition>();
            if (context is null)
                return members;

            var visitor = new MemberVisitor();
            foreach (var c in context.class_member_declarations().class_member_declaration())
            {
                var member = visitor.VisitClass_member_declaration(c);
                if (member is not null)
                    members.Add(member);
            }
            return members;
        }

        public static List<EnumMemberDefinition> FromContext(Enum_bodyContext? context)
        {
            if(context is null)
                return new List<EnumMemberDefinition>();

            return context.enum_member_declaration()
                .Select(c => new EnumMemberDefinition(
                    c.identifier().GetText(),
                    c.expression()?.GetText(),
                    AttributeGroups.FromContext(c.attributes())))
                .ToList();
        }
    }
}
