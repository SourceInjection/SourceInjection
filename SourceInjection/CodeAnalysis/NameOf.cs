namespace SourceInjection.CodeAnalysis
{
    public static class NameOf
    {
        public static string GenericIEnumerable { get; } = "System.Collections.Generic.IEnumerable";

        public static string IEnumerable { get; } = "System.Collections.IEnumerable";

        public static string LinqEnumerable { get; } = "System.Linq.Enumerable";

        public static string SourceInjectionEnumerable { get; } = "SourceInjection.Collections.Enumerable";

        public static string SourceInjectionArray { get; } = "SourceInjection.Collections.Array";

        public static string LinqSequenceEqual { get; } = $"{LinqEnumerable}.SequenceEqual";

        public static string SourceInjectionDeepSequenceEqual { get; } = $"{SourceInjectionEnumerable}.DeepSequenceEqual";

        public static string SourceInjectionCombinedHashCode { get; } = $"{SourceInjectionEnumerable}.CombinedHashCode";

        public static string SourceInjectionDeepCombinedHashCode { get; } = $"{SourceInjectionEnumerable}.DeepCombinedHashCode";

        public static string SourceInjectionArraySequenceEqual { get; } = $"{SourceInjectionArray}.SequenceEqual";

        public static string MaybeNullAttribute { get; } = "System.Diagnostics.CodeAnalysis.MaybeNullAttribute";

        public static string NotNullAttribute { get; } = "System.Diagnostics.CodeAnalysis.NotNullAttribute";

        public static string DisallowNullAttribute { get; } = "System.Diagnostics.CodeAnalysis.DisallowNullAttribute";

        public static string AllowNullAttribute { get; } = "System.Diagnostics.CodeAnalysis.AllowNullAttribute";
    }
}
