using static Aspects.Parsers.CSharp.Generated.CSharpParser;

namespace Aspects.Parsers.CSharp.Visitors.Common
{
    internal static class Members
    {
        public static List<MemberDefinition> FromContext(Struct_bodyContext? context)
        {
            if (context is null)
                return [];

            var members = new List<MemberDefinition>();
            // TODO
            return members;
        }

        public static List<MemberDefinition> FromContext(Class_bodyContext? context)
        {
            if (context is null)
                return [];

            var members = new List<MemberDefinition>();
            // TODO
            return members;
        }

        public static List<EnumMemberDefinition> FromContext(Enum_bodyContext? context)
        {

        }
    }
}
