using Aspects.Util;
using System;
using System.Linq;

namespace Aspects.SourceGenerators.Common
{
    internal static class Types
    {
        public static Predicate<TypeInfo> WithAttributeOfType<T>()
        {
            return (TypeInfo type) => type.Symbol.HasAttributeOfType<T>();
        }

        public static Predicate<TypeInfo> WithMembersWithAttributeOfType<T>()
        {
            return (TypeInfo type) => type.Symbol.GetMembers().Any(m => m.HasAttributeOfType<T>());
        }
    }
}
