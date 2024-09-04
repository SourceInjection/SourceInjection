using Aspects.Util;
using System.Collections;

namespace Aspects.Collections
{
    public static class Enumerable
    {
        /// <summary>
        /// Computes the hash code of nested <see cref="IEnumerable"/>s.
        /// </summary>
        /// <param name="en">The <see cref="IEnumerable"/> for which the hash code is computed.</param>
        /// <returns>The hash code of the <see cref="IEnumerable"/>.</returns>
        public static int DeepCombinedHashCode(this IEnumerable en)
        {
            if (en == null)
                return 0;
            return EnumerableExtensions.DeepCombinedHashCode(en);
        }

        /// <summary>
        /// Computes the hash code of <see cref="IEnumerable"/>s.
        /// </summary>
        /// <param name="en">The <see cref="IEnumerable"/> for which the hash code is computed.</param>
        /// <returns>The hash code of the <see cref="IEnumerable"/>.</returns>
        public static int CombinedHashCode(this IEnumerable en)
        {
            if (en == null)
                return 0;
            return EnumerableExtensions.CombinedHashCode(en);
        }

        /// <summary>
        /// Compares two <see cref="IEnumerable"/> on equality.
        /// </summary>
        /// <param name="a">The first collect<see cref="IEnumerable"/>.ion</param>
        /// <param name="b">The second <see cref="IEnumerable"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="IEnumerable"/>s are equal else <see langword="false"/>.</returns>
        public static bool DeepSequenceEqual(IEnumerable a, IEnumerable b)
        {
            return EnumerableExtensions.DeepSequenceEqual(a, b);
        }
    }
}
