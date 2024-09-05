namespace Aspects.SourceGenerators.Common
{
    internal class NameOf
    {
        /// <summary>
        /// System.Collections.Generic.IEnumerable
        /// </summary>
        public static string GenericIEnumerable { get; } = "System.Collections.Generic.IEnumerable";

        /// <summary>
        /// System.Collections.IEnumerable.
        /// </summary>
        public static string IEnumerable { get; } = typeof(System.Collections.IEnumerable).FullName;

        public static string LinqEnumerable { get; } = typeof(System.Linq.Enumerable).FullName;

        public static string LinqSequenceEqual { get; } = $"{LinqEnumerable}.{nameof(System.Linq.Enumerable.SequenceEqual)}";

        public static string AspectsEnumerable { get; } = typeof(Collections.Enumerable).FullName;

        public static string AspectsDeepSequenceEqual { get; } = $"{AspectsEnumerable}.{nameof(Collections.Enumerable.DeepSequenceEqual)}";

        public static string AspectsCombinedHashCode { get; } = $"{AspectsEnumerable}.{nameof(Collections.Enumerable.CombinedHashCode)}";

        public static string AspectsDeepCombinedHashCode { get; } = $"{AspectsEnumerable}.{nameof(Collections.Enumerable.DeepCombinedHashCode)}";

        public static string MaybeNullAttribute { get; } = "System.Diagnostics.CodeAnalysis.MaybeNullAttribute";

        public static string NotNullAttribute { get; } = "System.Diagnostics.CodeAnalysis.NotNullAttribute";
    }
}
