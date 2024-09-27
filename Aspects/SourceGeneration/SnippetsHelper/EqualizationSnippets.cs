using Aspects.CodeAnalysis;
using Aspects.SourceGeneration.Common;
using Aspects.SourceGeneration.DataMembers;
using Aspects.Util;
using Microsoft.CodeAnalysis;

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

        public static string EqualityCheck(DataMemberSymbolInfo member, string otherName, bool nullSafe, string comparer, bool isInequality)
        {
            if (!string.IsNullOrEmpty(comparer))
            {
                var suportsNullable = EqualityComparerInfo.EqualsSupportsNullable(comparer, member.Type);
                comparer = Reduce.ComparerName(member, comparer);

                if (!member.Type.IsReferenceType && member.Type.HasNullableAnnotation() && !suportsNullable)
                    return ComparerNullableNonReferenceTypeEquality(comparer, member.Name, otherName, nullSafe, isInequality);

                nullSafe = nullSafe && (member.Type.IsReferenceType || member.Type.HasNullableAnnotation());

                return ComparerEquality(comparer, member.Name, otherName, nullSafe, isInequality);
            }

            if (member.Type.CanUseEqualityOperatorsByDefault())
                return OperatorEquality(member.Name, otherName, isInequality);

            if (!member.Type.OverridesEquals())
            {
                if (member.Type.CanUseSequenceEquals())
                    return LinqSequenceEquality(member.Name, otherName, nullSafe, isInequality);

                if (member.Type is IArrayTypeSymbol arrayType && arrayType.Rank > 1)
                    return AspectsArrayEquality(member.Name, otherName, nullSafe, isInequality);

                if (member.Type.IsEnumerable())
                    return AspectsSequenceEquality(member.Name, otherName, nullSafe, isInequality);
            }
            nullSafe &= member.Type.IsReferenceType;

            return MethodEquality(member.Name, otherName, nullSafe, isInequality);
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
