using SourceInjection.CodeAnalysis;
using SourceInjection.SourceGeneration.Common;
using SourceInjection.SourceGeneration.DataMembers;
using SourceInjection.Util;

namespace SourceInjection.SourceGeneration.SnippetsHelper
{
    internal static class HashCodeSnippets
    {
        public static string ComparerHashCode(string name, string comparer, bool nullSafe)
            => HashCode(name, $"new {comparer}().GetHashCode({name})", nullSafe);

        public static string CombinedHashCode(string name, bool nullSafe)
            => HashCode(name, $"{NameOf.SourceInjectionCombinedHashCode}({name})", nullSafe);

        public static string DeepCombinedHashCode(string name, bool nullSafe)
            => HashCode(name, $"{NameOf.SourceInjectionDeepCombinedHashCode}({name})", nullSafe);

        public static string ComparerNullableNonReferenceTypeHashCode(string name, string comparer, bool nullSafe)
            => HashCode(name, $"new {comparer}().GetHashCode({name}.Value)", nullSafe);

        private static string HashCode(string name, string hashCodeCode, bool nullSafe)
        {
            return nullSafe
                ? $"{name} == null ? 0 : {hashCodeCode}"
                : hashCodeCode;
        }

        public static string GetHashCode(DataMemberSymbolInfo member, bool nullSafe, ComparerInfo comparer)
        {
            if (comparer != null)
            {
                var comparerName = Reduce.ComparerName(member, comparer.Name);

                if (!member.Type.IsReferenceType && member.Type.HasNullableAnnotation() && !comparer.IsNullSafe)
                    return ComparerNullableNonReferenceTypeHashCode(member.Name, comparerName, nullSafe);

                nullSafe = nullSafe && (member.Type.IsReferenceType || member.Type.HasNullableAnnotation());

                return ComparerHashCode(member.Name, comparerName, nullSafe);
            }

            if (!member.Type.OverridesGetHashCode())
            {
                if (member.Type.CanUseCombinedHashCode())
                    return CombinedHashCode(member.Name, nullSafe);
                if (member.Type.IsEnumerable())
                    return DeepCombinedHashCode(member.Name, nullSafe);
            }
            return member.Name;
        }
    }
}
