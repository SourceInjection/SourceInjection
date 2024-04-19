using Aspects.Util;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Aspects.Comparers
{
    public class DeepEnumerableEqualityComparer : IEqualityComparer<IEnumerable>
    {
        public bool Equals(IEnumerable x, IEnumerable y)
        {
            if (x is null)
                throw new ArgumentNullException(nameof(x));
            if(y is null)
                throw new ArgumentNullException(nameof(y));

            return x.DeepSequenceEqual(y);
        }

        public int GetHashCode(IEnumerable obj)
        {
            if(obj is null)
                throw new ArgumentNullException(nameof(obj));
            return obj.DeepCombinedHashCode();
        }
    }
}
