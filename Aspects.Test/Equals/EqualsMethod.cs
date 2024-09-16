using CompileUnits.CSharp;
using CompileUnit = Aspects.Test.CodeAnalysis.CompileUnit;

namespace Aspects.Test.Equals
{
    internal static class EqualsMethod
    {
        public static IMethod FromType<T>(bool useObjectMethod = false)
        {
            return FromType(typeof(T), useObjectMethod);
        }

        public static IMethod FromType(Type type, bool useObjectMethod = false)
        {
            var cu = CompileUnit.FromGeneratedCode<SGEquals>(type);
            var typeName = useObjectMethod ? "object" : type.Name;
            return cu.AllChildren()
                .OfType<IMethod>()
                .Single(m => IsTypedEqualsMethod(m, typeName));
        }

        private static bool IsTypedEqualsMethod(IMethod m, string typeName)
        {
            return m.Name == nameof(Equals)
                && m.Parameters.Count == 1
                && m.Parameters[0].Type.FormatedText.Trim('?') == typeName;
        }
    }
}
