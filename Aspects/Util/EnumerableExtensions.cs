using System;
using System.Collections;

namespace Aspects.Util
{
    internal static class EnumerableExtensions
    {
        public static int DeepCombinedHashCode(this IEnumerable col)
        {
            var hash = new HashCode();

            foreach (var item in col)
            {
                if (item is IEnumerable en)
                    hash.Add(DeepCombinedHashCode(en));
                else
                    hash.Add(item);
            }

            return hash.ToHashCode();
        }

        public static int CombinedHashCode(this IEnumerable col)
        {
            var hash = new HashCode();
            foreach (var item in col)
                hash.Add(item);
            return hash.ToHashCode();
        }

        public static bool DeepSequenceEqual(this IEnumerable col, IEnumerable other)
        {
            var colIt = col.GetEnumerator();
            var otherIt = other.GetEnumerator();

            while(colIt.MoveNext())
            {
                if(!otherIt.MoveNext() || 
                    !( colIt.Current == null && otherIt.Current == null
                    || colIt.Current?.Equals(otherIt.Current) == true
                    || colIt.Current is IEnumerable a && otherIt.Current is IEnumerable b && a.DeepSequenceEqual(b)))
                {
                    return DisposedReturn(false, colIt, otherIt);
                }
            }

            if (otherIt.MoveNext())
                return DisposedReturn(false, colIt, otherIt);
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
