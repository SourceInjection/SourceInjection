using Aspects.SourceGenerators;
using CompileUnits.CSharp;
using CompileUnit = Aspects.Test.CodeAnalysis.CompileUnit;

namespace Aspects.Test.Equals
{
    internal class EqualsMethod
    {
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

        public static string MemberOperatorEqualization(string memberName) => $"&& {memberName} == #i.{memberName}";

        public static string MemberEqualization(string memberName) => $"&& {memberName}.Equals(#i.{memberName})";

        public static string NullSafeMemberEqualization(string memberName) => $"&& ({memberName} == null && #i.{memberName} == null " +
            $"|| {memberName}?.Equals(#i.{memberName}) == true)";

        public static string LinqCollectionEqualization(string memberName) => $"&& System.Linq.Enumerable.SequenceEqual({memberName}, #i.{memberName})";

        public static string NullSafeLinqCollectionEqualization(string memberName) => $"&& ({memberName} == null && #i.{memberName} == null " +
            $"|| {memberName} != null && #i.{memberName} != null && System.Linq.Enumerable.SequenceEqual({memberName}, #i.{memberName}))";

        public static string AspectsCollectionEqualization(string memberName) => $"&& Aspects.Collections.Enumerable.SequenceEqual({memberName}, #i.{memberName})";
    }
}
