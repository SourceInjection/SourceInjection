using CompileUnits.CSharp;

namespace Aspects.Test.HashCode
{
    internal static class HashCodeMethod
    {
        public static IMethod FromType<T>() => FromType(typeof(T));

        public static IMethod FromType(Type type)
        {
            var cu = CodeAnalysis.CompileUnit.FromGeneratedCode<SGHashCode>(type);

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
