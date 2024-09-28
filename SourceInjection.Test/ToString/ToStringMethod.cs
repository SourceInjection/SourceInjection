using CompileUnits.CSharp;

namespace SourceInjection.Test.ToString
{
    internal static class ToStringMethod
    {
        public static IMethod FromType<T>() => FromType(typeof(T));

        public static IMethod FromType(Type type)
        {
            var cu = CodeAnalysis.CompileUnit.FromGeneratedCode<SGToString>(type);

            return cu.AllChildren()
                .OfType<IMethod>()
                .Single(IsToStringMethod);
        }

        private static bool IsToStringMethod(IMethod m)
        {
            return m.Name == nameof(ToString)
                && m.Parameters.Count == 0;
        }
    }
}
