using Aspects.SourceGenerators.Common;
using Aspects.Util;
using System;
using System.Linq;

namespace Aspects.SourceGenerators.Queries
{
    internal static class Types
    {
        /// <summary>
        /// Matches all <see cref="TypeInfo"/>s which have an <see cref="Attribute"/> of type T.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Attribute"/>.</typeparam>
        /// <returns>A <see cref="Predicate{T}"/> for <see cref="TypeInfo"/> which is fullfilled when 
        /// the <see cref="Type"/> has an <see cref="Attribute"/> of type T.</returns>
        public static Predicate<TypeInfo> WithAttributeOfType<T>()
        {
            return (type) => type.Symbol.HasAttributeOfType<T>();
        }

        /// <summary>
        /// Matches all <see cref="Type"/>s with members which have an <see cref="Attribute"/> of type T.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <returns>A <see cref="Predicate{T}"/> for <see cref="TypeInfo"/> which is fullfilled when 
        /// the <see cref="Type"/> has at least one <see cref="Attribute"/> of type T at any member.</returns>
        public static Predicate<TypeInfo> WithMembersWithAttributeOfType<T>()
        {
            return (type) => type.Symbol.GetMembers().Any(m => m.HasAttributeOfType<T>());
        }
    }
}
