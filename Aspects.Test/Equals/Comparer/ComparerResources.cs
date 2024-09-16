#pragma warning disable

namespace Aspects.Test.Equals.Comparer
{
    using NullSafety = Aspects.NullSafety;

    internal class ComparerBase<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y) => x.Equals(y);

        public int GetHashCode(T obj) => throw new NotImplementedException();
    }

    internal class NullableComparerBase<T> : IEqualityComparer<T>
    {
        public bool Equals(T? x, T? y)
        {
            if(x == null ^ y == null)
                return false;
            return x != null && x.Equals(y);
        }

        public int GetHashCode(T? obj)
        {
            throw new NotImplementedException();
        }
    }

    public partial class ClassWithMember_ThatHasCustomComparer_EqualsConfig
    {
        private class IntComparer : ComparerBase<int> { }

        [Equals(equalityComparer: typeof(IntComparer))]
        public int Property { get; }
    }

    [AutoEquals]
    public partial class ClassWithMember_ThatHasCustomComparer_ComparerAttribute
    {
        private class IntComparer : ComparerBase<int> { }

        [EqualityComparer(equalityComparer: typeof(IntComparer))]
        public int Property { get; }
    }
}

#pragma warning restore
