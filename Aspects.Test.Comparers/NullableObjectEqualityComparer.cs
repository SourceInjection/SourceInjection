namespace Aspects.Test.Comparers
{
    public class NullableObjectEqualityComparer : IEqualityComparer<object?>
    {
        public new bool Equals(object? x, object? y)
        {
            return x == null && y == null
                || x != null && y != null && x.Equals(y);
        }

        public int GetHashCode(object? obj)
        {
            return obj == null 
                ? 0
                : obj.GetHashCode();
        }
    }
}
