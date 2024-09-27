using Aspects.SourceGeneration.DataMembers;

namespace Aspects.SourceGeneration.SnippetsHelper
{
    internal static class Reduce
    {
        public static string ComparerName(DataMemberSymbolInfo member, string comparerName)
        {
            var containingTypeName = member.ContainingType.ToDisplayString();

            if (IsMemberOf(containingTypeName, comparerName))
                return comparerName.Substring(containingTypeName.Length + 1);

            return comparerName;
        }

        private static bool IsMemberOf(string containingTypeName, string typeUsage)
        {
            return typeUsage.StartsWith(containingTypeName)
                && typeUsage.Length > containingTypeName.Length
                && typeUsage[containingTypeName.Length] == '.';
        }
    }
}
