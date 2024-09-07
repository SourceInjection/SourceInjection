using Aspects.Attributes;

#pragma warning disable

namespace Aspects.Test.Equals.Comparer
{
    internal class ComparerBase<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y) => throw new NotImplementedException();

        public int GetHashCode(T obj) => throw new NotImplementedException();
    }

    public partial class ClassWithMember_ThatHasCustomComparer
    {
        private class IntComparer : ComparerBase<int> { }

        [Equals(equalityComparer: typeof(IntComparer))]
        public int Property { get; }
    }

    public partial class ClassWithNullableNonReferenceTypeMember_ThatHasNonNullableComparer
    {
        private class IntComparer : ComparerBase<int> { }

        [Equals(equalityComparer: typeof(IntComparer))]
        public int? Property { get; }
    }


    public partial class ClassWithNullableNonReferenceTypeMember_ThatHasNullableComparer
    {
        private class IntComparer : ComparerBase<int?> { }

        [Equals(equalityComparer: typeof(IntComparer))]
        public int? Property { get; }
    }
}

#pragma warning restore
