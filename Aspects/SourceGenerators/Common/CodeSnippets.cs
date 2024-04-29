
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

        /// <summary>
        /// Gets the full name of <see cref="IEnumerable"/>.
        /// </summary>
        public static string IEnumerableName => s_iEnumerableName;

        /// <summary>
        /// Creates a combined hash code method call which evaluates the hash code of <see cref="IEnumerable"/>s.
        /// </summary>
        /// <param name="argument">The name of the argument which is passed to the call.</param>
        /// <returns>A call of <see cref="Enumerable.CombinedHashCode(IEnumerable)"/>.</returns>
        public static string CombinedHashCodeMethod(string argument)
        {
            return $"{s_combinedHashCode}({argument})";
        }

        /// <summary>
        /// Creates a <see cref="IEnumerable"/> comparison which evaluates if two <see cref="IEnumerable"/>s are equal.
        /// </summary>
        /// <param name="arg1">The name of the first argument which is passed to the call.</param>
        /// <param name="arg2">The name of the second argument which is passed to the call.</param>
        /// <returns>A call of <see cref="Enumerable.SequenceEquals(IEnumerable, IEnumerable)"/>.</returns>
        public static string SequenceEqualsMethod(string arg1, string arg2)
        {
            return $"{s_sequenceEquals}({arg1}, {arg2})";
        }

        /// <summary>
        /// Evaluates the property name based on the name of a <see cref="IFieldSymbol"/> instance.<br/>
        /// Allows the following syntax:
        /// <code>
        /// '_'* ['a'..'z'] .*
        /// </code>
        /// And Produces a property without leading '_' and starting with an uppercase letter.
        /// </summary>
        /// <param name="field">The <see cref="IFieldSymbol"/> for wich the name is evaluated.</param>
        /// <returns>
        /// The name of the property.
        /// If the <see cref="IFieldSymbol"/>.Name does not match the pattern '_'* ['a'..'z'] .* the name of the field is returned.
        /// </returns>
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
