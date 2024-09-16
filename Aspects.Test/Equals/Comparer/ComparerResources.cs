#pragma warning disable

namespace Aspects.Test.Equals.Comparer
{
    using NullSafety = Aspects.NullSafety;

    internal static class ComparerResources
    {
        public static readonly Type[] Types =
        {
            typeof(ClassWithComparerAsMember_WithEqualsConfig),
            typeof(ClassWithComparerAsMember_WithComparerAttribute),
            typeof(ClassWithExternComparer_WithEqualsConfig),
            typeof(ClassWithExternComparer_WithComparerAttribute),
            typeof(ClassWithGenericComparer_WithEqualsConfig),
            typeof(ClassWithGenericComparer_WithComparerAttribute),
            typeof(ClassWithExternGenericComparer_WithEqualsConfig),
            typeof(ClassWithExternGenericComparer_WithComparerAttribute),
        };
    }

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

    public partial class ClassWithComparerAsMember_WithEqualsConfig
    {
        private class IntComparer : ComparerBase<int> { }

        [Equals(equalityComparer: typeof(IntComparer))]
        public int Property { get; }
    }

    [AutoEquals]
    public partial class ClassWithComparerAsMember_WithComparerAttribute
    {
        [EqualityComparer(equalityComparer: typeof(Comparers.IntEqualityComparer))]
        public int Property { get; }
    }

    public partial class ClassWithExternComparer_WithEqualsConfig
    {
        [Equals(equalityComparer: typeof(Comparers.IntEqualityComparer))]
        public int Property { get; }
    }

    [AutoEquals]
    public partial class ClassWithExternComparer_WithComparerAttribute
    {
        [EqualityComparer(equalityComparer: typeof(Comparers.IntEqualityComparer))]
        public int Property { get; }
    }

    public partial class ClassWithGenericComparer_WithEqualsConfig
    {
        [Equals(equalityComparer: typeof(ComparerBase<int>))]
        public int Property { get; }
    }

    [AutoEquals]
    public partial class ClassWithGenericComparer_WithComparerAttribute
    {
        [EqualityComparer(equalityComparer: typeof(ComparerBase<int>))]
        public int Property { get; }
    }

    public partial class ClassWithExternGenericComparer_WithEqualsConfig
    {
        [Equals(equalityComparer: typeof(Comparers.GenericEqualityComparer<int>))]
        public int Property { get; }
    }

    [AutoEquals]
    public partial class ClassWithExternGenericComparer_WithComparerAttribute
    {
        [EqualityComparer(equalityComparer: typeof(Comparers.GenericEqualityComparer<int>))]
        public int Property { get; }
    }
}

#pragma warning restore
