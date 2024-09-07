using Aspects.SourceGenerators.Common;

namespace Aspects.Test.Equals
{
    internal static class Equalization
    {
        public static string Operator(string memberName)
            => new EqualityCodeInfo(memberName, $"#i.{memberName}").OperatorEquality();

        public static string Equals(string memberName, bool nullSafe)
            => new EqualityCodeInfo(memberName, $"#i.{memberName}").MethodEquality(nullSafe);

        public static string LinqCollection(string memberName, bool nullSafe)
            => new EqualityCodeInfo(memberName, $"#i.{memberName}").LinqSequenceEquality(nullSafe);

        public static string AspectsCollection(string memberName, bool nullSafe)
            => new EqualityCodeInfo(memberName, $"#i.{memberName}").AspectsSequenceEquality(nullSafe);

        public static string AspectsArray(string memberName, bool nullSafe)
            => new EqualityCodeInfo(memberName, $"#i.{memberName}").AspectsArrayEquality(nullSafe);
    }
}
