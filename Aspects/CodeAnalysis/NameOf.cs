namespace Aspects.CodeAnalysis
{
    public static class NameOf
    {
        public static string GenericIEnumerable { get; } = "System.Collections.Generic.IEnumerable";

        public static string IEnumerable { get; } = "System.Collections.IEnumerable";

        public static string LinqEnumerable { get; } = "System.Linq.Enumerable";

        public static string AspectsEnumerable { get; } = "Aspects.Collections.Enumerable";

        public static string AspectsArray { get; } = "Aspects.Collections.Array";

        public static string LinqSequenceEqual { get; } = $"{LinqEnumerable}.SequenceEqual";

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
