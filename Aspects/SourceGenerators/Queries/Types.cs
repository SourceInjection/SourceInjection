using Aspects.SourceGenerators.Common;
using Aspects.Util;
using System;
using System.Linq;

namespace Aspects.SourceGenerators.Queries
{
    internal static class Types
    {
        public static Predicate<TypeInfo> WithAttributeOfType<T>()
        {
            return (type) => type.Symbol.HasAttributeOfType<T>();
        }

        public static Predicate<TypeInfo> WithMembersWithAttributeOfType<T>()
        {
            return (type) => type.Symbol.GetMembers().Any(m => m.HasAttributeOfType<T>());
        }
    }
}
