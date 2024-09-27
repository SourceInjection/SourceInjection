using Aspects.SourceGeneration.Common;
using Aspects.SourceGeneration.DataMembers;
using Aspects.Util.SymbolExtensions;

namespace Aspects.SourceGeneration.SnippetsHelper
{
    internal static class HashCodeSnippets
    {
        public static string ComparerHashCode(string name, string comparer, bool nullSafe)
            => HashCode(name, $"new {comparer}().GetHashCode({name})", nullSafe);

        public static string CombinedHashCode(string name, bool nullSafe)
            => HashCode(name, $"{NameOf.AspectsCombinedHashCode}({name})", nullSafe);

        public static string DeepCombinedHashCode(string name, bool nullSafe)
            => HashCode(name, $"{NameOf.AspectsDeepCombinedHashCode}({name})", nullSafe);

        public static string ComparerNullableNonReferenceTypeHashCode(string name, string comparer, bool nullSafe)
            => HashCode(name, $"new {comparer}().GetHashCode({name}.Value)", nullSafe);

        private static string HashCode(string name, string hashCodeCode, bool nullSafe)
        {
            return nullSafe
                ? $"{name} == null ? 0 : {hashCodeCode}"
                : hashCodeCode;
        }

        public static string GetHashCode(DataMemberSymbolInfo member, bool nullSafe, string comparer)
        {
            if (!string.IsNullOrEmpty(comparer))
            {
                var suportsNullable = EqualityComparerInfo.HashCodeSupportsNullable(comparer, member.Type);
                comparer = Reduce.ComparerName(member, comparer);

                if (!member.Type.IsReferenceType && member.Type.HasNullableAnnotation() && !suportsNullable)
                    return ComparerNullableNonReferenceTypeHashCode(member.Name, comparer, nullSafe);

                nullSafe = nullSafe && (member.Type.IsReferenceType || member.Type.HasNullableAnnotation());

                return ComparerHashCode(member.Name, comparer, nullSafe);
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
