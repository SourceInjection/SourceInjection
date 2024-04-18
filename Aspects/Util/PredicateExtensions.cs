using System;

namespace Aspects.Util
{
    internal static class PredicateExtensions
    {
        public static Predicate<T> Or<T>(this Predicate<T> lhs, Predicate<T> rhs)
        {
            return (T value) => lhs(value) || rhs(value);
        }

        public static Predicate<T> And<T>(this Predicate<T> lhs, Predicate<T> rhs)
        {
            return (T value) => lhs(value) && rhs(value);
        }
    }
}
