using Aspects.SourceGenerators.Base.DataMembers;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System.Text;

namespace Aspects.SourceGenerators.Common
{
    internal static class Code
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
        {
            var info = new EqualityCodeInfo(member.Name, otherName, true);
            return EqualityFromInfo(member, info, nullSafe, comparer);
        }

        public static string EqualityCheck(DataMemberSymbolInfo member, string otherName, bool nullSafe, string comparer)
        {
            var info = new EqualityCodeInfo(member.Name, otherName);
            return EqualityFromInfo(member, info, nullSafe, comparer);
        }

        private static string EqualityFromInfo(DataMemberSymbolInfo member, EqualityCodeInfo codeInfo, bool nullSafe, string comparer)
        {
            if (!string.IsNullOrEmpty(comparer))
            {
                if (!member.Type.IsReferenceType && member.Type.HasNullableAnnotation())
                    return codeInfo.ComparerNullableNonReferenceTypeEquality(comparer, nullSafe);
                return codeInfo.ComparerEquality(comparer, nullSafe && member.Type.IsReferenceType);
            }

            if (member.Type.CanUseEqualityOperatorsByDefault())
                return codeInfo.OperatorEquality();

            if (!member.Type.OverridesEquals())
            {
                if (member.Type.CanUseSequenceEquals())
                    return codeInfo.LinqSequenceEquality(nullSafe);

                if (member.Type is IArrayTypeSymbol arrayType && arrayType.Rank > 1)
                    return codeInfo.AspectsArrayEquality(nullSafe);

                if (member.Type.IsEnumerable())
                    return codeInfo.AspectsSequenceEquality(nullSafe);
            }
            return codeInfo.MethodEquality(member.Type.IsReferenceType && nullSafe);
        }


        public static string GetHashCode(ITypeSymbol type, string name, bool nullSafe, string comparer)
        {
            if (!string.IsNullOrEmpty(comparer))
            {
                if (!type.IsReferenceType && type.HasNullableAnnotation())
                    return new HashCodeCodeInfo(name).ComparerNullableNonReferenceTypeHashCode(comparer, nullSafe);
                return new HashCodeCodeInfo(name).ComparerHashCode(comparer, nullSafe && type.IsReferenceType);
            }

            if (!type.OverridesGetHashCode())
            {
                var info = new HashCodeCodeInfo(name);

                if (type.CanUseCombinedHashCode())
                    return info.CombinedHashCode(nullSafe);
                if (type.IsEnumerable())
                    return info.DeepCombinedHashCode(nullSafe);
            }
            return name;
        }
    }
}
