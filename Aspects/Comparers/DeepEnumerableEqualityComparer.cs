using System.Collections;
using System.Collections.Generic;

namespace Aspects.Comparers
{
    public class DeepEnumerableEqualityComparer : IEqualityComparer<IEnumerable>
    {
        public bool Equals(IEnumerable x, IEnumerable y)
        {
            return Enumerable.SequenceEquals(x, y);
        }

        public int GetHashCode(IEnumerable obj)
        {
            return Enumerable.CombinedHashCode(obj);
        }
    }
}
