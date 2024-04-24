using Aspects.Util;
using System.Collections;

namespace Aspects.Collections
{
    public static class Enumerable
    {
        public static int CombinedHashCode(IEnumerable en)
        {
            if (en is null)
                return 0;
            return en.DeepCombinedHashCode();
        }

        public static bool SequenceEquals(IEnumerable a, IEnumerable b)
        {
            if (a is null && b is null)
                return true;
            if (!(a is null) && !(b is null))
                return a.DeepSequenceEqual(b);
            return false;
        }
    }
}
