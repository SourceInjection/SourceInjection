using Aspects.SourceGenerators.Common;
using Aspects.Util;
using System;
using System.Linq;

namespace Aspects.SourceGenerators.Queries
{
    internal static class Types
    {
        /// <summary>
        /// Matches all types which have an attribute of type T
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <returns>A predicate on TypeInfo which is fullfilled when the type has an attribute of type T</returns>
        public static Predicate<TypeInfo> WithAttributeOfType<T>()
        {
            return (type) => type.Symbol.HasAttributeOfType<T>();
        }

        /// <summary>
        /// Matches all types with members which have an attribute of type T
        /// </summary>
        /// <typeparam name="T">The type of the attribute</typeparam>
        /// <returns>A predicate on TypeInfo which is fullfilled when the type has at least one attribute of type T at any member</returns>
        public static Predicate<TypeInfo> WithMembersWithAttributeOfType<T>()
        {
            return (type) => type.Symbol.GetMembers().Any(m => m.HasAttributeOfType<T>());
        }
    }
}
