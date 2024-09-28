namespace SourceInjection.Test.Comparers
{
    public class NullableIntEqualityComparer : IEqualityComparer<int?>
    {
        public bool Equals(int? x, int? y)
        {
            return x == null && y == null
                || x != null && y != null && x.Value.Equals(y.Value);
        }

        public int GetHashCode(int? obj)
        {
            return obj == null
                ? 0
                : obj.Value;
        }
    }
}
