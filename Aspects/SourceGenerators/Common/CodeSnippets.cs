using Aspects.Util;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Aspects.SourceGenerators.Common
{
    internal static class CodeSnippets
    {
        private static readonly string s_enumerableClass = typeof(Collections.Enumerable).FullName;
        private static readonly string s_safeSequenceEquals = $"{s_enumerableClass}.{nameof(Collections.Enumerable.SequenceEqual)}";
        private static readonly string s_safeCombinedHashCode = $"{s_enumerableClass}.{nameof(Collections.Enumerable.CombinedHashCode)}";

        private static readonly string s_iEnumerableName = typeof(System.Collections.IEnumerable).FullName;
        private static readonly string s_genericIEnumerableName= "System.Collections.Generic.IEnumerable";

        private static readonly string s_linqEnumerableName = typeof(System.Linq.Enumerable).FullName;
        private static readonly string s_linqSequenceEquals = $"{s_linqEnumerableName}.{nameof(System.Linq.Enumerable.SequenceEqual)}";

        /// <summary>
        /// System.Collections.Generic.<see cref="IEnumerable"/>.
        /// </summary>
        public static string GenericIEnumerableName => s_genericIEnumerableName;


        /// <summary>
        /// System.Collections.<see cref="IEnumerable"/>.
        /// </summary>
        public static string IEnumerableName => s_iEnumerableName;

        /// <summary>
        /// Creates a local variable and resolves conflicts with already given members.
        /// E.g. desired with name 'temp' but field with name 'temp' already exists then it will check for 'temp0', 'temp1' and so on.
        /// </summary>
        /// <param name="type">The type symbol from wich the members are scanned.</param>
        /// <param name="name">The desired name.</param>
        /// <param name="alreadyCreatedVariables">if multiple variables are needed you may wan't to take care of them too.</param>
        /// <returns>A variable name without conflicts within its containing type.</returns>
        public static string CreateVariable(INamedTypeSymbol type, string name = "temp", System.Collections.Generic.ISet<string> alreadyCreatedVariables = null)
        {
            var raw = name;
            var i = 0;

            while (type.GetMembers(name).Length > 0 || alreadyCreatedVariables != null && alreadyCreatedVariables.Contains(name))
                name = $"{raw}{i++}";

            return name;
        }

        /// <summary>
        /// Creates a combined hash code method call which evaluates the hash code of <see cref="IEnumerable"/>s.
        /// </summary>
        /// <param name="argument">The name of the argument which is passed to the call.</param>
        /// <returns>A call of Aspects.<see cref="Collections.Enumerable.CombinedHashCode(IEnumerable)"/>.</returns>
        public static string SafeCombinedHashCodeMethod(string argument)
        {
            return $"{s_safeCombinedHashCode}({argument})";
        }

        /// <summary>
        /// Creates a <see cref="IEnumerable"/> comparison which evaluates if two <see cref="IEnumerable"/>s are equal.
        /// </summary>
        /// <param name="arg1">The name of the first argument which is passed to the call.</param>
        /// <param name="arg2">The name of the second argument which is passed to the call.</param>
        /// <returns>A call of Aspects.<see cref="Collections.Enumerable.SequenceEquals(IEnumerable, IEnumerable)"/>.</returns>
        public static string SafeSequenceEqualsMethod(string arg1, string arg2)
        {
            return $"{s_safeSequenceEquals}({arg1}, {arg2})";
        }


        /// <summary>
        /// Creates a <see cref="IEnumerable"/> comparison which evaluates if two <see cref="IEnumerable"/>s are equal.
        /// </summary>
        /// <param name="arg1">The name of the first argument which is passed to the call.</param>
        /// <param name="arg2">The name of the second argument which is passed to the call.</param>
        /// <returns>A call of <see cref="System.Linq.Enumerable.SequenceEquals(IEnumerable, IEnumerable)"/>.</returns>
        public static string LinqSequenceEqualsMethod(string arg1, string arg2)
        {
            return $"{s_linqSequenceEquals}({arg1}, {arg2})";
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
            return PropertyNameFromField(field.Name);
        }

        /// <summary>
        /// Evaluates the property name based on the parameter <paramref name="fieldName"/>.<br/>
        /// Allows the following syntax:
        /// <code>
        /// '_'* ['a'..'z'] .*
        /// </code>
        /// And Produces a property without leading '_' and starting with an uppercase letter.
        /// </summary>
        /// <param name="fieldName">The name of the field for wich the name is evaluated.</param>
        /// <returns>
        /// The name of the property.
        /// If <param name="fieldName"/> does not match the pattern '_'* ['a'..'z'] .* the name of the field is returned.
        /// </returns>
        public static string PropertyNameFromField(string fieldName)
        {
            while (fieldName.Length > 0 && fieldName[0] == '_')
                fieldName = fieldName.Substring(1);

            if (fieldName.Length > 0 && fieldName[0] >= 'a' && fieldName[0] <= 'z')
                fieldName = char.ToUpper(fieldName[0]) + fieldName.Substring(1);

            return fieldName;
        }

        /// <summary>
        /// Indents a string with tabulators (two spaces)-
        /// </summary>
        /// <param name="value">The value to indent.</param>
        /// <param name="tabCount">The number of tabulators used.</param>
        /// <returns>An indented string.</returns>
        public static string Indent(string value = "", int tabCount = 1)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < tabCount; i++)
                sb.Append("  ");
            sb.Append(value);
            return sb.ToString();
        }

        /// <summary>
        /// A code snippet for a inequality comparison.<br/>
        /// - '!=' for types which have inequality operator by default (e.g. simple types)<br/>
        /// - !Equals for types which are neither reference types nor have an default inequality operator (e.g. structs)<br/>
        /// - !Aspects.Collections.Enumerable.SequenceEquals for collections<br/>
        /// - a safe version of inequality for every other type if <param name="nullSafe"/> is enabled
        /// - Equals else
        /// </summary>
        /// <param name="type">The type of both identifiers.</param>
        /// <param name="nameA">The first identifier.</param>
        /// <param name="nameB">The second identifier.</param>
        /// <param name="nullSafe">Enables null safety</param>
        /// <returns>The inequality code.</returns>
        public static string InequalityCheck(ITypeSymbol type, string nameA, string nameB, bool nullSafe)
        {
            if (type.CanUseEqualityOperatorsByDefault())
                return $"{nameA} != {nameB}";

            if (!type.OverridesEquals())
            {
                if (type.CanBeComparedBySequenceEqual())
                {
                    return nullSafe
                        ? NullSafeSequenceInequality(nameA, nameB)
                        : $"!{s_linqSequenceEquals}({nameA}, {nameB})";
                }
                if (type.IsEnumerable())
                    return $"!{SafeSequenceEqualsMethod(nameA, nameB)}";
            }

            if (!type.IsReferenceType || !nullSafe)
                return $"!{nameA}.{nameof(Equals)}({nameB})";

            return NullSafeInequality(nameA, nameB);
        }

        /// <summary>
        /// A code snippet for a equality comparison.<br/>
        /// - '==' for types which have equality operator by default (e.g. simple types)<br/>
        /// - Equals for types which are neither reference types nor have an default inequality operator (e.g. structs)<br/>
        /// - Aspects.Collections.Enumerable.SequenceEquals for collections<br/>
        /// - a safe version of equality for every other type if <param name="nullSafe"/> is enabled
        /// - Equals else
        /// </summary>
        /// <param name="type">The type of both identifiers.</param>
        /// <param name="nameA">The first identifier.</param>
        /// <param name="nameB">The second identifier.</param>
        /// <param name="nullSafe">Enables null safety</param>
        /// <returns>The equality code.</returns>
        public static string EqualityCheck(ITypeSymbol type, string nameA, string nameB, bool nullSafe)
        {
            if (type.CanUseEqualityOperatorsByDefault())
                return $"{nameA} == {nameB}";

            if (!type.OverridesEquals())
            {
                if (type.CanBeComparedBySequenceEqual())
                {
                    return nullSafe
                        ? NullSafeSequenceEquality(nameA, nameB)
                        : $"{s_linqSequenceEquals}({nameA}, {nameB})";
                }
                if (type.IsEnumerable())
                    return $"{SafeSequenceEqualsMethod(nameA, nameB)}";
            }

            if (!type.IsReferenceType || !nullSafe)
                return $"{nameA}.{nameof(Equals)}({nameB})";

            return NullSafeEquality(nameA, nameB);
        }

        private static string NullSafeInequality(string nameA, string nameB)
        {
            return $"{nameA} != null && !{nameA}.{nameof(Equals)}({nameB}) || {nameA} == null && {nameB} != null";
        }

        private static string NullSafeEquality(string nameA, string nameB)
        {
            return $"{nameA} == null && {nameB} == null || {nameA}?.{nameof(Equals)}({nameB}) == true";
        }

        private static string NullSafeSequenceEquality(string nameA, string nameB)
        {
            return $"{nameA} == null && {nameB} == null || {nameA} != null && {nameB} != null && {s_linqSequenceEquals}({nameA}, {nameB})";
        }

        private static string NullSafeSequenceInequality(string nameA, string nameB)
        {
            return $"{nameA} == null && {nameB} != null || {nameA} != null && {nameB} == null || {nameA} != null && {nameB} != null && {s_linqSequenceEquals}({nameA}, {nameB})";
        }


    }
}
