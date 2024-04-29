using System;

namespace Aspects.Util
{
    internal static class PredicateExtensions
    {
        /// <summary>
        /// Combines two predicates with an disjunctional operator (or)
        /// </summary>
        /// <typeparam name="T">The type of the generic predicate arguments</typeparam>
        /// <param name="lhs">The left hand side predicate</param>
        /// <param name="rhs">The right hand side predicate</param>
        /// <returns>A new predicate that combines two predicates with an disjunctional operator (or)</returns>
        public static Predicate<T> Or<T>(this Predicate<T> lhs, Predicate<T> rhs)
        {
            return (T value) => lhs(value) || rhs(value);
        }
    }
}
