using System.Text;

namespace Aspects.Test.ToString
{
    internal static class ToString
    {
        public static string Body(Type type, params string[] memberNames)
        {
            var sb = new StringBuilder($"{{ return $\"({type.Name})");
            if(memberNames.Length > 0)
            {
                sb.Append("{{");
                sb.Append($"{memberNames[0]}: {{{memberNames[0]}}}");

                for (int i = 1; i < memberNames.Length; i++)
                    sb.Append($", {memberNames[i]}: {{{memberNames[i]}}}");
                sb.Append("}}");
            }
            sb.Append("\"; }");
            return sb.ToString();
        }
    }
}
