using Aspects.SourceGenerators;
using CompileUnits.CSharp;
using CompileUnit = Aspects.Test.CodeAnalysis.CompileUnit;

namespace Aspects.Test.CompileTime.Equals
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
    }
}
