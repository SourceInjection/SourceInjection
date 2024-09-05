using Aspects.SourceGenerators;
using Aspects.SourceGenerators.Common;
using CompileUnits.CSharp;
using CompileUnit = Aspects.Test.CodeAnalysis.CompileUnit;

namespace Aspects.Test.Equals
{
    internal class EqualsMethod
    {
        private static readonly string LinqSequenceEqual = $"{typeof(Enumerable).FullName}.{nameof(Enumerable.SequenceEqual)}";
        private static readonly string AspectsDeepSequenceEqual = $"{typeof(Collections.Enumerable).FullName}.{nameof(Collections.Enumerable.DeepSequenceEqual)}";
        private static readonly string AspectsArraySequenceEqual = $"{typeof(Collections.Array).FullName}.{nameof(Collections.Array.SequenceEqual)}";

        public static IMethod FromType<T>()
        {
            var cu = CompileUnit.FromGeneratedCode<EqualsSourceGenerator, T>();

            return cu.AllChildren()
                .OfType<IMethod>()
                .Single(IsEqualsMethod);
        }

        private static bool IsEqualsMethod(IMethod m)
        {
            return m.Name == nameof(Equals)
                && m.Parameters.Count == 1
                && m.Parameters[0].Type.FormatedText is "object" or "object?";
        }

        public static string OperatorEqualization(string memberName) 
            => new EqualityCodeInfo(memberName, $"#i.{memberName}").OperatorEquality();

        public static string EqualsEqualization(string memberName)
            => new EqualityCodeInfo(memberName, $"#i.{memberName}").MethodEquality(false);

        public static string NullSafeEqualsEqualization(string memberName)
            => new EqualityCodeInfo(memberName, $"#i.{memberName}").MethodEquality(true);

        public static string LinqCollectionEqualization(string memberName)
            => new EqualityCodeInfo(memberName, $"#i.{memberName}").LinqSequenceEquality(false);

        public static string NullSafeLinqCollectionEqualization(string memberName)
            => new EqualityCodeInfo(memberName, $"#i.{memberName}").LinqSequenceEquality(true);

        public static string AspectsCollectionEqualization(string memberName)
            => new EqualityCodeInfo(memberName, $"#i.{memberName}").AspectsSequenceEquality(false);

        public static string AspectsArrayEqualization(string memberName)
            => new EqualityCodeInfo(memberName, $"#i.{memberName}").AspectsArrayEquality(false);
    }
}
