using Aspects.Util;
using System;
using System.Collections;

namespace Aspects
{
    public static class Enumerable
    {
        public static int CombinedHashCode(this IEnumerable en)
        {
            if (en is null)
                throw new ArgumentNullException(nameof(en));
            return EnumerableExtensions.CombinedHashCode(en);
        }

        public static int DeepCombinedHashCode(this IEnumerable en)
        {
            if(en is null)
                throw new ArgumentNullException(nameof(en));
            return EnumerableExtensions.DeepCombinedHashCode(en);
        }

        public static bool SequenceEquals(this IEnumerable a, IEnumerable b)
        {
            if (a is null)
                throw new ArgumentNullException(nameof(a));
            if (b is null)
                throw new ArgumentNullException(nameof(b));
            return EnumerableExtensions.SequenceEqual(a, b);
        }

        public static bool DeepSequenceEquals(this IEnumerable a, IEnumerable b)
        {
            if (a is null)
                throw new ArgumentNullException(nameof(a));
            if (b is null)
                throw new ArgumentNullException(nameof(b));
            return EnumerableExtensions.DeepSequenceEqual(a, b);
        }
    }
}
