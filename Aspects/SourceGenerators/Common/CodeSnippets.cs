
using Aspects.Collections;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System.Collections;
using System.Text;

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

        /// <summary>
        /// Indents a string with tabulators (\t)-
        /// </summary>
        /// <param name="value">The value to indent.</param>
        /// <param name="tabCount">The number of tabulators used.</param>
        /// <returns>An indented string.</returns>
        public static string Indent(string value, int tabCount)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < tabCount; i++)
                sb.Append('\t');
            sb.Append(value);
            return sb.ToString();
        }

        /// <summary>
        /// A code snippet for a inequality comparison.<br/>
        /// - '!=' for types which have inequality operator by default (e.g. simple types)<br/>
        /// - !Equals for types which are neither reference types nor have an default inequality operator (e.g. structs)<br/>
        /// - !Aspects.Collections.Enumerable.SequenceEquals for collections<br/>
        /// - a safe version of inequality for every other type
        /// </summary>
        /// <param name="type">The type of the two identifiers.</param>
        /// <param name="nameA">The first identifier.</param>
        /// <param name="nameB">The second identifier.</param>
        /// <returns>The inequality code.</returns>
        public static string InequalityCheck(ITypeSymbol type, string nameA, string nameB)
        {
            if (type.CanUseEqualityOperatorsByDefault())
                return $"{nameA} != {nameB}";

            if (!type.IsReferenceType)
                return $"!{nameA}.{nameof(Equals)}({nameB})";

            if (type.IsEnumerable() && !type.OverridesEquals())
                return $"!{SequenceEqualsMethod(nameA, nameB)}";

            return $"!({nameA} is null) && !{nameA}.{nameof(Equals)}({nameB}) || {nameA} is null && !({nameB} is null)";
        }

        /// <summary>
        /// A code snippet for a equality comparison.<br/>
        /// - '==' for types which have equality operator by default (e.g. simple types)<br/>
        /// - Equals for types which are neither reference types nor have an default inequality operator (e.g. structs)<br/>
        /// - Aspects.Collections.Enumerable.SequenceEquals for collections<br/>
        /// - a safe version of equality for every other type
        /// </summary>
        /// <param name="type">The type of the two identifiers.</param>
        /// <param name="nameA">The first identifier.</param>
        /// <param name="nameB">The second identifier.</param>
        /// <returns>The equality code.</returns>
        public static string EqualityCheck(ITypeSymbol type, string nameA, string nameB)
        {
            if (type.CanUseEqualityOperatorsByDefault())
                return $"{nameA} == {nameB}";

            if (!type.IsReferenceType)
                return $"{nameA}.{nameof(Equals)}({nameB})";

            if (type.IsEnumerable() && !type.OverridesEquals())
                return $"{SequenceEqualsMethod(nameA, nameB)}";

            return $"{nameA} is null && {nameB} is null || {nameA}?.{nameof(Equals)}({nameB}) is true";
        }
    }
}
