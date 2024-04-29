
using Aspects.Collections;
using Microsoft.CodeAnalysis;
using System.Collections;

namespace Aspects.SourceGenerators.Common
{
    internal static class CodeSnippets
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

        public static string PropertyNameFromField(IFieldSymbol field)
        {
            var fieldName = field.Name;
            while (fieldName.Length > 0 && fieldName[0] == '_')
                fieldName = fieldName.Substring(1);

            if (fieldName.Length > 0 && fieldName[0] >= 'a' && fieldName[0] <= 'z')
                fieldName = char.ToUpper(fieldName[0]) + fieldName.Substring(1);

            return fieldName;
        }
    }
}
