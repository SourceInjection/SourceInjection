using System;
using System.Collections;

namespace Aspects.Util
{
    internal static class EnumerableExtensions
    {
        public static int DeepCombinedHashCode(this IEnumerable col)
        {
            var hash = 0;
            const int prime = 31;

            foreach (var item in col)
            {
                int value;
                if (item is IEnumerable en)
                    value = DeepCombinedHashCode(en);
                else
                    value = item?.GetHashCode() ?? 0;

                hash = unchecked(hash * prime + value);
            }
            return hash;
        }

        public static bool DeepSequenceEqual(this IEnumerable col, IEnumerable other)
        {
            var colIt = col.GetEnumerator();
            var otherIt = other.GetEnumerator();

            while(colIt.MoveNext())
            {
                if(!otherIt.MoveNext() || 
                    !( colIt.Current is null && otherIt.Current is null
                    || colIt.Current?.Equals(otherIt.Current) is true
                    || colIt.Current is IEnumerable a && otherIt.Current is IEnumerable b && a.DeepSequenceEqual(b)))
                {
                    return DisposedReturn(false, colIt, otherIt);
                }
            }

            return DisposedReturn(true, colIt, otherIt);
        }

        private static T DisposedReturn<T>(T returnValue, params object[] toDispose)
        {
            for(var i = 0; i < toDispose.Length; i++)
                DisposeAny(toDispose[i]);
            return returnValue;
        }

        private static void DisposeAny(object obj)
        {
            if (obj is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
