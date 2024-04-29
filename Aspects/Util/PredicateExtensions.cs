using System;

namespace Aspects.Util
{
    internal static class PredicateExtensions
    {
        /// <summary>
        /// Combines two <see cref="Predicate{T}"/>s with an disjunctional operator (or).
        /// </summary>
        /// <typeparam name="T">The type of both <see cref="Predicate{T}"/>s.</typeparam>
        /// <param name="lhs">The left hand side <see cref="Predicate{T}"/>.</param>
        /// <param name="rhs">The right hand side <see cref="Predicate{T}"/>.</param>
        /// <returns>A new <see cref="Predicate{T}"/> 
        /// that combines two <see cref="Predicate{T}"/>s with an disjunctional operator (or).</returns>
        public static Predicate<T> Or<T>(this Predicate<T> lhs, Predicate<T> rhs)
        {
            return (T value) => lhs(value) || rhs(value);
        }
    }
}
