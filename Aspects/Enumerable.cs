using Aspects.Util;
using System.Collections;

namespace Aspects
{
    public static class Enumerable
    {
        public static int CombinedHashCode(this IEnumerable en)
        {
            if (en is null)
                return 0;
            return EnumerableExtensions.DeepCombinedHashCode(en);
        }

        public static bool SequenceEquals(this IEnumerable a, IEnumerable b)
        {
            if (a is null && b is null)
                return true;
            if (!(a is null) && !(b is null))
                return EnumerableExtensions.DeepSequenceEqual(a, b);
            return false;
        }
    }
}
