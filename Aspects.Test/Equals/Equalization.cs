using Aspects.SourceGeneration.Common;
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
            => CodeInfo(memberName).ComparerEquality(Test.Comparer.FromMember(containingType, memberName), nullSafe);

        public static string Comparer(string comparer, string memberName, bool nullSafe)
            => CodeInfo(memberName).ComparerEquality(comparer, nullSafe);

        public static string ComparerNullableNonReferenceType(Type containingType, string memberName, bool nullSafe)
            => CodeInfo(memberName).ComparerNullableNonReferenceTypeEquality(Test.Comparer.FromMember(containingType, memberName), nullSafe);

        public static string ComparerNullableNonReferenceType(string comparer, string memberName, bool nullSafe)
            => CodeInfo(memberName).ComparerNullableNonReferenceTypeEquality(comparer, nullSafe);

        public static Action Build<TType>(string propertyName)
        {
            return () =>
            {
                var lhs = (TType?)Activator.CreateInstance(typeof(TType));
                var rhs = (TType?)Activator.CreateInstance(typeof(TType));

                var prop = typeof(TType).GetProperty(propertyName);

                if (lhs is null || rhs is null || prop is null)
                    throw new TypeLoadException($"could not get necessary information.");

                prop.SetValue(lhs, null);
                prop.SetValue(rhs, null);

                lhs.Equals(rhs);
            };
        }

        private static EqualityCodeInfo CodeInfo(string memberName) 
            => new (memberName, $"#i.{memberName}");
    }
}
