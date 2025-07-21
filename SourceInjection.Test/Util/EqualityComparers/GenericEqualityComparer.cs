namespace SourceInjection.Test.Util.EqualityComparers
{
    public class GenericEqualityComparer<T> : IEqualityComparer<T>
    {
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public bool Equals(T x, T y)
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        {
            return x is null && y is null
                || x is not null && y is not null && x.Equals(y);
        }

        public int GetHashCode(T obj)
        {
            return obj is null ? 0 : obj.GetHashCode();
        }
    }
}
