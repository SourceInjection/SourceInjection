namespace Aspects.Common.Paths
{
    internal class NameOf
    {
        public static string GenericIEnumerable { get; } = "System.Collections.Generic.IEnumerable";

        public static string IEnumerable { get; } = typeof(System.Collections.IEnumerable).FullName;

        public static string LinqEnumerable { get; } = typeof(System.Linq.Enumerable).FullName;

        public static string LinqSequenceEqual { get; } = $"{LinqEnumerable}.{nameof(System.Linq.Enumerable.SequenceEqual)}";

        public static string AspectsEnumerable { get; } = typeof(Collections.Enumerable).FullName;

        public static string AspectsArray { get; } = typeof(Collections.Array).FullName;

        public static string AspectsDeepSequenceEqual { get; } = $"{AspectsEnumerable}.{nameof(Collections.Enumerable.DeepSequenceEqual)}";

        public static string AspectsCombinedHashCode { get; } = $"{AspectsEnumerable}.{nameof(Collections.Enumerable.CombinedHashCode)}";

        public static string AspectsDeepCombinedHashCode { get; } = $"{AspectsEnumerable}.{nameof(Collections.Enumerable.DeepCombinedHashCode)}";

        public static string AspectsArraySequenceEqual { get; } = $"{AspectsArray}.{nameof(Collections.Array.SequenceEqual)}";

        public static string MaybeNullAttribute { get; } = "System.Diagnostics.CodeAnalysis.MaybeNullAttribute";

        public static string NotNullAttribute { get; } = "System.Diagnostics.CodeAnalysis.NotNullAttribute";
    }
}
