using Aspects.SourceGenerators.Common;
using System.Reflection;

namespace Aspects.Test.Equals
{
    internal static class Equalization
    {
        public static string Operator(string memberName)
            => CodeInfo(memberName).OperatorEquality();

        public static string Equals(string memberName, bool nullSafe)
            => CodeInfo(memberName).MethodEquality(nullSafe);

        public static string LinqCollection(string memberName, bool nullSafe)
            => CodeInfo(memberName).LinqSequenceEquality(nullSafe);

        public static string AspectsCollection(string memberName, bool nullSafe)
            => CodeInfo(memberName).AspectsSequenceEquality(nullSafe);

        public static string AspectsArray(string memberName, bool nullSafe)
            => CodeInfo(memberName).AspectsArrayEquality(nullSafe);

        public static string Comparer(Type containingType, string memberName, bool nullSafe)
            => CodeInfo(memberName).ComparerEquality(GetComparer(containingType, memberName), nullSafe);

        public static string ComparerNullableNonReferenceType(Type containingType, string memberName, bool nullSafe)
            => CodeInfo(memberName).ComparerNullableNonReferenceTypeEquality(GetComparer(containingType, memberName), nullSafe);

        private static EqualityCodeInfo CodeInfo(string memberName) 
            => new (memberName, $"#i.{memberName}");

        private static string GetComparer(Type type, string member)
        {
            var comparerName = type.GetMember(member, BindingFlags.Instance | BindingFlags.Public)[0].GetCustomAttributesData()
                .First(attData => attData.ConstructorArguments.Any(ca => ca.ArgumentType == typeof(Type)))!.ConstructorArguments
                .First(ca => ca.ArgumentType == typeof(Type)).ToString();

            if (comparerName.StartsWith("typeof("))
                comparerName = comparerName[7..];
            comparerName = comparerName.TrimEnd(')');

            return comparerName.Replace('+', '.');
        }
    }
}
