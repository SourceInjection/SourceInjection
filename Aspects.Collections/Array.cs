namespace Aspects.Collections
{
    public static class Array
    {
        /// <summary>
        /// Compared two <see cref="System.Array"/>s for equality no mather if multidimensional or not.
        /// </summary>
        /// <param name="array">The first <see cref="System.Array"/>.</param>
        /// <param name="other">The other <see cref="System.Array"/>.</param>
        /// <returns><langword cref="true"/>if the <see cref="System.Array"/>s are equal else <see langword="false"/></returns>

        public static bool SequenceEqual(this System.Array array, System.Array other)
        {
            if (array.Rank != other.Rank)
                return false;
            for (int i = 0; i < array.Rank; i++)
            {
                if (array.GetLength(i) != other.GetLength(i))
                    return false;
            }
            return Enumerable.DeepSequenceEqual(array, other);
        }
    }
}
