using Aspects.Util.SymbolExtensions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aspects.Common
{
    internal static class Types
    {
        private readonly static Dictionary<string, INamedTypeSymbol> _allTypes = new Dictionary<string, INamedTypeSymbol>(1024);

        public static INamedTypeSymbol Get(string name)
        {
            if (_allTypes.TryGetValue(name, out var value))
                return value;
            return null;
        }

        public static void Add(string name, INamedTypeSymbol type)
        {
            _allTypes.Add(name, type);
        }

        /// <summary>
        /// Matches all <see cref="TypeInfo"/>s which have an <see cref="Attribute"/> of type T.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Attribute"/>.</typeparam>
        /// <returns>A <see cref="Predicate{T}"/> for <see cref="TypeInfo"/> which is fullfilled when 
        /// the <see cref="Type"/> has an <see cref="Attribute"/> of type T.</returns>
        public static Predicate<INamedTypeSymbol> WithAttributeOfType<T>()
        {
            return (symbol) => symbol.HasAttributeOfType<T>();
        }

        /// <summary>
        /// Matches all <see cref="Type"/>s with members which have an <see cref="Attribute"/> of type T.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <returns>A <see cref="Predicate{T}"/> for <see cref="TypeInfo"/> which is fullfilled when 
        /// the <see cref="Type"/> has at least one <see cref="Attribute"/> of type T at any member.</returns>
        public static Predicate<INamedTypeSymbol> WithMembersWithAttributeOfType<T>()
        {
            return (symbol) => symbol.GetMembers().Any(m => m.HasAttributeOfType<T>());
        }
    }
}
