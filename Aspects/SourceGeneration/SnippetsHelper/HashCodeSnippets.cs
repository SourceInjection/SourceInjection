using Aspects.Common;

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
    }
}
