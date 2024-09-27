using System.Text;

namespace Aspects.Test.ToString
{
    internal static class ToString
    {
        public static string Member(string memberName, string? label = null, string? format = null, bool coalesce = false)
        {
            var memberLabel = string.IsNullOrEmpty(label) 
                ? memberName : label;

            var coalesceOp = coalesce ? "?" : string.Empty;
            var memberValue = string.IsNullOrEmpty(format)
                ? $"{{{memberName}}}"
                : $"{{{memberName}{coalesceOp}.ToString(\"{format}\")}}";

            return $"{memberLabel}: {memberValue}";
        }

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
