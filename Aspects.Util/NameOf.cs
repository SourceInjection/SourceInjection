namespace Aspects.Util
{
    internal static class NameOf
    {
        public static string GenericIEnumerable { get; } = "System.Collections.Generic.IEnumerable";

        public static string IEnumerable { get; } = typeof(System.Collections.IEnumerable).FullName;

        public static string LinqEnumerable { get; } = typeof(System.Linq.Enumerable).FullName;

        public static string LinqSequenceEqual { get; } = $"{LinqEnumerable}.{nameof(System.Linq.Enumerable.SequenceEqual)}";

        public static string AspectsEnumerable { get; } = "Aspects.Util.EnumerableExtensions";

        public static string AspectsArray { get; } = "Aspects.Util.ArrayExtensions";

        public static string AspectsDeepSequenceEqual { get; } = $"{AspectsEnumerable}.DeepSequenceEqual";

        public static string AspectsCombinedHashCode { get; } = $"{AspectsEnumerable}.CombinedHashCode";

        public static string AspectsDeepCombinedHashCode { get; } = $"{AspectsEnumerable}.DeepCombinedHashCode";

        public static string AspectsArraySequenceEqual { get; } = $"{AspectsArray}.SequenceEqual";

        public static string MaybeNullAttribute { get; } = "System.Diagnostics.CodeAnalysis.MaybeNullAttribute";

        public static string NotNullAttribute { get; } = "System.Diagnostics.CodeAnalysis.NotNullAttribute";

        public static string DisallowNullAttribute { get; } = "System.Diagnostics.CodeAnalysis.DisallowNullAttribute";

        public static string AllowNullAttribute { get; } = "System.Diagnostics.CodeAnalysis.AllowNullAttribute";
    }
}
