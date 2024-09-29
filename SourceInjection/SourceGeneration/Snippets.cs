﻿using SourceInjection.SourceGeneration.DataMembers;
using SourceInjection.SourceGeneration.SnippetsHelper;
using Microsoft.CodeAnalysis;
using SourceInjection.SourceGeneration.Common;

namespace SourceInjection.SourceGeneration
{
    internal static class Snippets
    {
        public static string PropertyNameFromField(IFieldSymbol field)
            => PropertySnippets.PropertyNameFromField(field.Name);

        public static string InequalityCheck(DataMemberSymbolInfo member, string otherName, bool nullSafe, ComparerInfo comparer)
            => EqualizationSnippets.EqualityCheck(member, otherName, nullSafe, comparer, true);

        public static string EqualityCheck(DataMemberSymbolInfo member, string otherName, bool nullSafe, ComparerInfo comparer)
            => EqualizationSnippets.EqualityCheck(member, otherName, nullSafe, comparer, false);

        public static string GetHashCode(DataMemberSymbolInfo member, bool nullSafe, ComparerInfo comparer)
            => HashCodeSnippets.GetHashCode(member, nullSafe, comparer);

        public static string MemberToString(DataMemberSymbolInfo member, string label, string format)
            => ToStringSnippets.MemberToString(member.Name, label, format);

        public static string UnconflictingVariable(INamedTypeSymbol type, string name = "temp")
        {
            var raw = name;
            var i = 1;

            while (type.GetMembers(name).Length > 0)
                name = $"{raw}{i++}";

            return name;
        }
    }
}