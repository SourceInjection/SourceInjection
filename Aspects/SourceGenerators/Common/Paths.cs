namespace Aspects.SourceGenerators.Common
{
    internal class Paths
    {
        private static readonly string s_aspectsEnumerable = typeof(Collections.Enumerable).FullName;
        private static readonly string s_aspectsDeepCombinedHashCode = $"{s_aspectsEnumerable}.{nameof(Collections.Enumerable.DeepCombinedHashCode)}";
        private static readonly string s_iEnumerableName = typeof(System.Collections.IEnumerable).FullName;
        private static readonly string s_genericIEnumerableName = "System.Collections.Generic.IEnumerable";

        /// <summary>
        /// System.Collections.Generic.<see cref="IEnumerable"/>.
        /// </summary>
        public static string GenericIEnumerableName => s_genericIEnumerableName;

        /// <summary>
        /// System.Collections.<see cref="IEnumerable"/>.
        /// </summary>
        public static string IEnumerableName => s_iEnumerableName;

        public static string LinqEnumerable { get; } = typeof(System.Linq.Enumerable).FullName;

        public static string LinqSequenceEqual { get; } = $"{LinqEnumerable}.{nameof(System.Linq.Enumerable.SequenceEqual)}";

        public static string AspectsEnumerable { get; } = typeof(Collections.Enumerable).FullName;

        public static string AspectsSequenceEqual { get; } = $"{AspectsEnumerable}.{nameof(Collections.Enumerable.DeepSequenceEqual)}";

        public static string AspectsDeepCombinedHashCode { get; } = $"{AspectsEnumerable}.{nameof(Collections.Enumerable.DeepCombinedHashCode)}";
    }
}
