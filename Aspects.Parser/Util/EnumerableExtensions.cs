
namespace Aspects.Parsers.Util
{
    internal static class EnumerableExtensions
    {
        public static int CombinedHashCode<T>(this IEnumerable<T> en)
        {
            var hash = new HashCode();
            foreach (var item in en)
                hash.Add(item);

            return hash.ToHashCode();
        }
    }
}
