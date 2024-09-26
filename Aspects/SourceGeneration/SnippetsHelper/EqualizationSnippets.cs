using Aspects.SourceGeneration.Common;

namespace Aspects.SourceGeneration.SnippetsHelper
{
    internal static class EqualizationSnippets
    {
        public static string LinqSequenceEquality(string nameA, string nameB, bool nullSafe, bool isInequality)
        {
            return nullSafe
                ? NullSafeSequenceEquality(NameOf.LinqSequenceEqual, nameA, nameB, isInequality)
                : SequenceEquality(NameOf.LinqSequenceEqual, nameA, nameB, isInequality);
        }

        public static string AspectsSequenceEquality(string nameA, string nameB, bool nullSafe, bool isInequality)
        {
            return nullSafe
                ? NullSafeSequenceEquality(NameOf.AspectsDeepSequenceEqual, nameA, nameB, isInequality)
                : SequenceEquality(NameOf.AspectsDeepSequenceEqual, nameA, nameB, isInequality);
        }

        public static string MethodEquality(string nameA, string nameB, bool nullSafe, bool isInequality)
        {
            var methodCode = $"{nameA}.{nameof(Equals)}({nameB})";
            return nullSafe
                ? Equality(methodCode, nameA, nameB, isInequality)
                : MayInversed(methodCode, isInequality);
        }

        public static string AspectsArrayEquality(string nameA, string nameB, bool nullSafe, bool isInequality)
        {
            return nullSafe
                ? NullSafeSequenceEquality(NameOf.AspectsArraySequenceEqual, nameA, nameB, isInequality)
                : SequenceEquality(NameOf.AspectsArraySequenceEqual, nameA, nameB, isInequality);
        }

        public static string OperatorEquality(string nameA, string nameB, bool isInequality)
        {
            return isInequality
                ? $"{nameA} != {nameB}"
                : $"{nameA} == {nameB}";
        }

        public static string ComparerEquality(string comparer, string nameA, string nameB, bool nullSafe, bool isInequality)
        {
            var comparerCode = $"new {comparer}().Equals({nameA}, {nameB})";
            return nullSafe
                ? Equality(comparerCode, nameA, nameB, isInequality)
                : MayInversed(comparerCode, isInequality);
        }

        public static string ComparerNullableNonReferenceTypeEquality(string comparer, string nameA, string nameB, bool nullSafe, bool isInequality)
        {
            var comparerCode = $"new {comparer}().Equals({nameA}.Value, {nameB}.Value)";
            return nullSafe
                ? Equality(comparerCode, nameA, nameB, isInequality)
                : MayInversed(comparerCode, isInequality);
        }

        private static string MayInversed(string s, bool isInequality) => isInequality ? '!' + s : s;

        private static string SequenceEquality(string method, string nameA, string nameB, bool isInequality)
            => MayInversed($"{method}({nameA}, {nameB})", isInequality);

        private static string NullSafeSequenceEquality(string method, string nameA, string nameB, bool _isInequality)
        {
            var methodCode = $"{method}({nameA}, {nameB})";
            return _isInequality
                ? $"{nameA} == null ^ {nameB} == null || {nameA} != null && {nameA} != {nameB} && !{methodCode}"
                : $"{nameA} == {nameB} || {nameA} != null && {nameB} != null && {methodCode}";
        }

        private static string Equality(string methodComparison, string nameA, string nameB, bool isInequality)
        {
            return isInequality
                ? $"{nameA} == null ^ {nameB} == null || {nameA} != null && !{methodComparison}"
                : $"{nameA} == null && {nameB} == null || {nameA} != null && {nameB} != null && {methodComparison}";
        }
    }
}
