using System;
using System.Collections;

namespace Aspects.Util
{
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// Computes the hash code of <see cref="IEnumerable"/>s.
        /// </summary>
        /// <param name="col">The <see cref="IEnumerable"/> for which the hash code is computed.</param>
        /// <returns>The hash code of the <see cref="IEnumerable"/>.</returns>
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

        /// <summary>
        /// Compares two <see cref="IEnumerable"/> on equality.
        /// </summary>
        /// <param name="col">The first collect<see cref="IEnumerable"/>.ion</param>
        /// <param name="other">The second <see cref="IEnumerable"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="IEnumerable"/>s are equal else <see langword="false"/>.</returns>
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
