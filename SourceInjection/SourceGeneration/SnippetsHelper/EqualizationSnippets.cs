﻿using SourceInjection.CodeAnalysis;
using SourceInjection.SourceGeneration.Common;
using SourceInjection.SourceGeneration.DataMembers;
using SourceInjection.Util;
using Microsoft.CodeAnalysis;

namespace SourceInjection.SourceGeneration.SnippetsHelper
{
    internal static class EqualizationSnippets
    {
        public static string LinqSequenceEquality(string nameA, string nameB, bool nullSafe, bool isInequality)
        {
            return nullSafe
                ? NullSafeSequenceEquality(NameOf.LinqSequenceEqual, nameA, nameB, isInequality)
                : SequenceEquality(NameOf.LinqSequenceEqual, nameA, nameB, isInequality);
        }

        public static string SourceInjectionSequenceEquality(string nameA, string nameB, bool nullSafe, bool isInequality)
        {
            return nullSafe
                ? NullSafeSequenceEquality(NameOf.SourceInjectionDeepSequenceEqual, nameA, nameB, isInequality)
                : SequenceEquality(NameOf.SourceInjectionDeepSequenceEqual, nameA, nameB, isInequality);
        }

        public static string MethodEquality(string nameA, string nameB, bool nullSafe, bool isInequality)
        {
            var methodCode = $"{nameA}.{nameof(Equals)}({nameB})";
            return nullSafe
                ? Equality(methodCode, nameA, nameB, isInequality)
                : MayNegated(methodCode, isInequality);
        }

        public static string SourceInjectionArrayEquality(string nameA, string nameB, bool nullSafe, bool isInequality)
        {
            return nullSafe
                ? NullSafeSequenceEquality(NameOf.SourceInjectionArraySequenceEqual, nameA, nameB, isInequality)
                : SequenceEquality(NameOf.SourceInjectionArraySequenceEqual, nameA, nameB, isInequality);
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
                : MayNegated(comparerCode, isInequality);
        }

        public static string ComparerNullableNonReferenceTypeEquality(string comparer, string nameA, string nameB, bool nullSafe, bool isInequality)
        {
            var comparerCode = $"new {comparer}().Equals({nameA}.Value, {nameB}.Value)";
            return nullSafe
                ? Equality(comparerCode, nameA, nameB, isInequality)
                : MayNegated(comparerCode, isInequality);
        }

        public static string EqualityCheck(DataMemberSymbolInfo member, string otherName, bool nullSafe, EqualityComparerInfo comparer, bool isInequality)
        {
            if (comparer != null)
            {
                var comparerName = Reduce.ComparerName(member, comparer.Name);

                if (!member.Type.IsReferenceType && member.Type.HasNullableAnnotation() && !comparer.EqualsSupportsNullable)
                    return ComparerNullableNonReferenceTypeEquality(comparerName, member.Name, otherName, nullSafe, isInequality);

                nullSafe = nullSafe && (member.Type.IsReferenceType || member.Type.HasNullableAnnotation());

                return ComparerEquality(comparerName, member.Name, otherName, nullSafe, isInequality);
            }

            if (member.Type.CanUseEqualityOperatorsByDefault())
                return OperatorEquality(member.Name, otherName, isInequality);

            if (!member.Type.OverridesEquals())
            {
                if (member.Type.CanUseSequenceEquals())
                    return LinqSequenceEquality(member.Name, otherName, nullSafe, isInequality);

                if (member.Type is IArrayTypeSymbol arrayType && arrayType.Rank > 1)
                    return SourceInjectionArrayEquality(member.Name, otherName, nullSafe, isInequality);

                if (member.Type.IsEnumerable())
                    return SourceInjectionSequenceEquality(member.Name, otherName, nullSafe, isInequality);
            }
            nullSafe &= member.Type.IsReferenceType;

            return MethodEquality(member.Name, otherName, nullSafe, isInequality);
        }

        private static string MayNegated(string s, bool isInequality) => isInequality ? '!' + s : s;

        private static string SequenceEquality(string method, string nameA, string nameB, bool isInequality)
            => MayNegated($"{method}({nameA}, {nameB})", isInequality);

        private static string NullSafeSequenceEquality(string method, string nameA, string nameB, bool isInequality)
        {
            var methodCode = $"{method}({nameA}, {nameB})";
            return isInequality
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
