
using Aspects.Collections;
using System.Collections;

namespace Aspects.SourceGenerators.Common
{
    internal static class Output
    {
        private static readonly string s_enumerableClass = typeof(Enumerable).FullName;
        private static readonly string s_sequenceEquals = $"{s_enumerableClass}.{nameof(Enumerable.SequenceEquals)}";
        private static readonly string s_combinedHashCode = $"{s_enumerableClass}.{nameof(Enumerable.CombinedHashCode)}";
        private static readonly string s_iEnumerableName = typeof(IEnumerable).FullName;

        public static string IEnumerableName => s_iEnumerableName;

        public static string CombinedHashCodeMethod(string argument)
        {
            return $"{s_combinedHashCode}({argument})";
        }

        public static string SequenceEqualsMethod(string arg1, string arg2)
        {
            return $"{s_sequenceEquals}({arg1}, {arg2})";
        }
    }
}
