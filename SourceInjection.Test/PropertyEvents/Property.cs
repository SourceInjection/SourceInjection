using CompileUnits.CSharp;

namespace SourceInjection.Test.PropertyEvents
{
    internal static class Property
    {
        public static IProperty FromType<T>(string name) => FromType(typeof(T), name);

        public static IProperty FromType(Type type, string name)
        {
            var cu = CodeAnalysis.CompileUnit.FromGeneratedCode<SGProperty>(type);

            return cu.AllChildren()
                .OfType<IProperty>()
                .Single(p => p.Name == name);
        }
    }
}
