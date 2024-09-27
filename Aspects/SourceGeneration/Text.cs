using System.Text;

namespace Aspects.SourceGeneration
{
    internal static class Text
    {
        public static string Indent(string value = "", int tabCount = 1)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < tabCount; i++)
                sb.Append("  ");
            sb.Append(value);
            return sb.ToString();
        }
    }
}
