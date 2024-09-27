using Aspects.SourceGeneration.SnippetsHelper;
using System.Text;

namespace Aspects.Test.ToString
{
    internal static class ToString
    {
        public static string Member(string memberName, string? label = null, string? format = null)
            => ToStringSnippets.MemberToString(memberName, label, format);
        
        public static string Body(Type type, params string[] memberToString)
        {
            var sb = new StringBuilder($"{{ return $\"({type.Name})");
            if(memberToString.Length > 0)
            {
                sb.Append("{{");
                sb.Append(memberToString[0]);

                for (int i = 1; i < memberToString.Length; i++)
                    sb.Append($", {memberToString[i]}");
                sb.Append("}}");
            }
            sb.Append("\"; }");
            return sb.ToString();
        }
    }
}
