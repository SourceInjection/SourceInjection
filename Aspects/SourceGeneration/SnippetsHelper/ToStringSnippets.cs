namespace Aspects.SourceGeneration.SnippetsHelper
{
    internal class ToStringSnippets
    {
        public static string MemberToString(string member, string label, string format, bool coalesce)
        {
            return $"{MemberLabel(member, label)}: {MemberValue(member, format, coalesce)}";
        }

        private static string MemberLabel(string member, string label)
        {
            if (string.IsNullOrEmpty(label))
                return member;
            return label;
        }

        private static string MemberValue(string member, string format, bool coalesce)
        {
            if (string.IsNullOrEmpty(format))
                return $"{{{member}}}";
            var coalesceOp = coalesce ? "?" : string.Empty;
            return $"{{{member}{coalesceOp}.ToString(\"{format}\")}}";
        }
    }
}
