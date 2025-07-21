#pragma warning disable

namespace SourceInjection.Test.Equals.Comparer
{
    using SourceInjection.Test.Util.EqualityComparers;
    using NullSafety = SourceInjection.NullSafety;

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

        [EqualityComparer(typeof(IntComparer))]
        [Equals]
        public int Property { get; }
    }

    [AutoEquals]
    public partial class ClassWithComparerAsMember_WithComparerAttribute
    {
        [EqualityComparer(equalityComparer: typeof(IntEqualityComparer))]
        public int Property { get; }
    }

    public partial class ClassWithExternComparer_WithEqualsConfig
    {
        [EqualityComparer(typeof(IntEqualityComparer))]
        [Equals]
        public int Property { get; }
    }

    [AutoEquals]
    public partial class ClassWithExternComparer_WithComparerAttribute
    {
        [EqualityComparer(typeof(IntEqualityComparer))]
        [Equals]
        public int Property { get; }
    }

    public partial class ClassWithGenericComparer_WithEqualsConfig
    {
        [EqualityComparer(typeof(ComparerBase<int>))]
        [Equals]
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
        [EqualityComparer(equalityComparer: typeof(GenericEqualityComparer<int>))]
        [Equals]
        public int Property { get; }
    }

    [AutoEquals]
    public partial class ClassWithExternGenericComparer_WithComparerAttribute
    {
        [EqualityComparer(equalityComparer: typeof(GenericEqualityComparer<int>))]
        public int Property { get; }
    }
}

#pragma warning restore
