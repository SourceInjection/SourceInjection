using System.Diagnostics.CodeAnalysis;

#pragma warning disable
namespace Aspects.Test.HashCode.Comparer
{
    internal class ComparerBase<T> : IEqualityComparer<T>
    {
        public bool Equals(T? x, T? y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            throw new NotImplementedException();
        }
    }

    public partial class ClassWithIntPropertyAndNonNullableComparer
    {
        private class Comparer : ComparerBase<int> { }

        [HashCode(typeof(Comparer))]
        public int Int { get; }
    }

    public partial class ClassWithIntPropertyAndNullableComparer
    {
        private class Comparer : ComparerBase<int?> { }

        [HashCode(typeof(Comparer))]
        public int Int { get; }
    }

    public partial class ClassWithNullableIntPropertyAndNullableComparer
    {
        private class Comparer : ComparerBase<int?> { }

        [HashCode(typeof(Comparer))]
        public int? Int { get; }
    }

    public partial class ClassWithNullableIntPropertyAndNonNullableComparer
    {
        private class Comparer : ComparerBase<int> { }

        [HashCode(typeof(Comparer))]
        public int? Int { get; }
    }
}

#pragma warning enable
