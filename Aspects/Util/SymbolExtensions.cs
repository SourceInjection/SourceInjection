using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using Aspects.SourceGenerators.Common;

namespace Aspects.Util
{
    internal static class SymbolExtensions
    {
        /// <summary>
        /// Checks if the <see cref="ISymbol"/> has an <see cref="Attribute"/> of type T.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="symbol">The <see cref="ISymbol"/> which is checked if it has an <see cref="Attribute"/> of type T.</param>
        /// <returns><see langword="true"/> if the <see cref="ISymbol"/> has an <see cref="Attribute"/> of type T else <see langword="false"/>.</returns>
        public static bool HasAttributeOfType<T>(this ISymbol symbol)
        {
            return symbol.AttributesOfType<T>().Any();
        }

        /// <summary>
        /// Gets all <see cref="AttributeData"/> of type T within a <see cref="ISymbol"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the <see cref="Attribute"/>.</typeparam>
        /// <param name="symbol">The <see cref="ISymbol"/> for which the <see cref="AttributeData"/> are evaluated.</param>
        /// <returns>A <see cref="IEnumerable"/> of <see cref="AttributeData"/> where the associated <see cref="Attribute"/> is of type T.</returns>
        public static IEnumerable<AttributeData> AttributesOfType<T>(this ISymbol symbol)
        {
            var name = typeof(T).FullName;
            return symbol.GetAttributes().Where(a =>
                a.AttributeClass.Inheritance().Any(ac => ac.ToDisplayString() == name)
                || a.AttributeClass.AllInterfaces.Any(i => i.ToDisplayString() == name));
        }

        /// <summary>
        /// Checks if the given <see cref="ISymbol"/> has a System.Diagnostic.Analysis.NotNull attribute.
        /// </summary>
        /// <param name="symbol">The <see cref="ISymbol"/> to be checked.</param>
        /// <returns><see langword="true"/> if the <see cref="ISymbol"/> has a System.Diagnostic.Analysis.NotNull attribute else <see langword="false"/>.</returns>
        public static bool HasNotNullAttribute(this ISymbol symbol)
        {
            return symbol.GetAttributes()
                .Any(a => a.AttributeClass.ToDisplayString() == NameOf.NotNullAttribute);
        }

        /// <summary>
        /// Checks if the given <see cref="ISymbol"/> has a System.Diagnostic.Analysis.MaybeNull attribute.
        /// </summary>
        /// <param name="symbol">The <see cref="ISymbol"/> to be checked.</param>
        /// <returns><see langword="true"/> if the <see cref="ISymbol"/> has a System.Diagnostic.Analysis.MaybeNull attribute else <see langword="false"/>.</returns>
        public static bool HasMaybeNullAttribute(this ISymbol symbol)
        {
            return symbol.GetAttributes()
                .Any(a => a.AttributeClass.ToDisplayString() == NameOf.MaybeNullAttribute);
        }
    }
}
