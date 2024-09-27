using Aspects.SourceGeneration.Common;
using Aspects.SourceGeneration.DataMembers;
using Aspects.SourceGeneration.SnippetsHelper;
using Aspects.Util.SymbolExtensions;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Aspects.SourceGeneration
{
    internal static class Snippets
    {
        public static string CreateUnconflictingVariable(INamedTypeSymbol type, string name = "temp")
        {
            var raw = name;
            var i = 1;

            while (type.GetMembers(name).Length > 0)
                name = $"{raw}{i++}";

            return name;
        }

        public static string PropertyNameFromField(IFieldSymbol field)
        {
            return PropertyNameFromField(field.Name);
        }

        public static string PropertyNameFromField(string fieldName)
        {
            while (fieldName.Length > 0 && fieldName[0] == '_')
                fieldName = fieldName.Substring(1);

            if (fieldName.Length > 0 && fieldName[0] >= 'a' && fieldName[0] <= 'z')
                fieldName = char.ToUpper(fieldName[0]) + fieldName.Substring(1);

            return fieldName;
        }

        public static string Indent(string value = "", int tabCount = 1)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < tabCount; i++)
                sb.Append("  ");
            sb.Append(value);
            return sb.ToString();
        }

        public static string InequalityCheck(DataMemberSymbolInfo member, string otherName, bool nullSafe, string comparer)
            => EqualityCheck(member, otherName, nullSafe, comparer, true);


        public static string EqualityCheck(DataMemberSymbolInfo member, string otherName, bool nullSafe, string comparer)
            => EqualityCheck(member, otherName, nullSafe, comparer, false);

        private static string EqualityCheck(DataMemberSymbolInfo member, string otherName, bool nullSafe, string comparer, bool isInequality)
        {
            if (!string.IsNullOrEmpty(comparer))
            {
                var suportsNullable = EqualityComparerInfo.EqualsSupportsNullable(comparer, member.Type);
                comparer = ReduceComparerName(member, comparer);

                if (!member.Type.IsReferenceType && member.Type.HasNullableAnnotation() && !suportsNullable)
                    return EqualizationSnippets.ComparerNullableNonReferenceTypeEquality(comparer, member.Name, otherName, nullSafe, isInequality);

                nullSafe = nullSafe && (member.Type.IsReferenceType || member.Type.HasNullableAnnotation());

                return EqualizationSnippets.ComparerEquality(comparer, member.Name, otherName, nullSafe, isInequality);
            }

            if (member.Type.CanUseEqualityOperatorsByDefault())
                return EqualizationSnippets.OperatorEquality(member.Name, otherName, isInequality);

            if (!member.Type.OverridesEquals())
            {
                if (member.Type.CanUseSequenceEquals())
                    return EqualizationSnippets.LinqSequenceEquality(member.Name, otherName, nullSafe, isInequality);

                if (member.Type is IArrayTypeSymbol arrayType && arrayType.Rank > 1)
                    return EqualizationSnippets.AspectsArrayEquality(member.Name, otherName, nullSafe, isInequality);

                if (member.Type.IsEnumerable())
                    return EqualizationSnippets.AspectsSequenceEquality(member.Name, otherName, nullSafe, isInequality);
            }
            nullSafe &= member.Type.IsReferenceType;

            return EqualizationSnippets.MethodEquality(member.Name, otherName, nullSafe, isInequality);
        }

        public static string GetHashCode(DataMemberSymbolInfo member, bool nullSafe, string comparer)
        {
            if (!string.IsNullOrEmpty(comparer))
            {
                var suportsNullable = EqualityComparerInfo.HashCodeSupportsNullable(comparer, member.Type);
                comparer = ReduceComparerName(member, comparer);

                if (!member.Type.IsReferenceType && member.Type.HasNullableAnnotation() && !suportsNullable)
                    return HashCodeSnippets.ComparerNullableNonReferenceTypeHashCode(member.Name, comparer, nullSafe);

                nullSafe = nullSafe && (member.Type.IsReferenceType || member.Type.HasNullableAnnotation());
                return HashCodeSnippets.ComparerHashCode(member.Name, comparer, nullSafe);
            }

            if (!member.Type.OverridesGetHashCode())
            {
                if (member.Type.CanUseCombinedHashCode())
                    return HashCodeSnippets.CombinedHashCode(member.Name, nullSafe);
                if (member.Type.IsEnumerable())
                    return HashCodeSnippets.DeepCombinedHashCode(member.Name,nullSafe);
            }
            return member.Name;
        }

        private static string ReduceComparerName(DataMemberSymbolInfo member, string comparerName)
        {
            var containingTypeName = member.ContainingType.ToDisplayString();

            if (IsMemberOf(containingTypeName, comparerName))
                return comparerName.Substring(containingTypeName.Length + 1);

            return comparerName;
        }

        private static bool IsMemberOf(string containingTypeName, string typeUsage)
        {
            return typeUsage.StartsWith(containingTypeName)
                && typeUsage.Length > containingTypeName.Length
                && typeUsage[containingTypeName.Length] == '.';
        }
    }
}
