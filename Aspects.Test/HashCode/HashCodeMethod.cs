using CompileUnits.CSharp;

namespace Aspects.Test.HashCode
{
    internal static class HashCodeMethod
    {
        public static IMethod FromType<T>()
        {
            var cu = CodeAnalysis.CompileUnit.FromGeneratedCode<HashCodeSourceGenerator, T>();

            return cu.AllChildren()
                .OfType<IMethod>()
                .Single(IsGetHashCodeMethod);
        }

        private static bool IsGetHashCodeMethod(IMethod m)
        {
            return m.Name == nameof(GetHashCode)
                && m.Parameters.Count == 0;
        }
    }
}
