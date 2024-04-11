using System;
using System.Linq;

namespace Aspects.SourceGenerators.Common
{
    internal static class Types
    {
        public static Predicate<TypeInfo> With<T>() where T : Attribute
        {
            var name = typeof(T).FullName;
            return (TypeInfo type) => 
                type.Symbol.GetAttributes().Any(a => a.AttributeClass.ToDisplayString() == name);
        }

        public static Predicate<TypeInfo> WithMembersWith<T>() where T : Attribute
        {
            var name = typeof(T).FullName;
            return (TypeInfo type) =>
                type.Symbol.GetMembers().SelectMany(m => m.GetAttributes()).Any(a => a.AttributeClass.ToDisplayString() == name);
        }
    }
}
