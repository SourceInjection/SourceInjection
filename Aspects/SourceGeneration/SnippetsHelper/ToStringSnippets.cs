namespace Aspects.SourceGeneration.SnippetsHelper
{
    internal static class ToStringSnippets
    {
        public static string MemberToString(string member, string label, string format)
        {
            return $"{MemberLabel(member, label)}: {MemberValue(member, format)}";
        }

        private static string MemberLabel(string member, string label)
        {
            if (string.IsNullOrEmpty(label))
                return member;
            return label;
        }

        private static string MemberValue(string member, string format)
        {
            if (string.IsNullOrEmpty(format))
                return $"{{{member}}}";
            return $"{{{member}:{format}}}";
        }
    }
}
