#pragma warning disable

namespace SourceInjection.Test.HashCode.Comparer
{
    using SourceInjection.Test.Util.EqualityComparers;
    using NullSafety = SourceInjection.NullSafety;

    internal static class ComparerResources
    {
        public static readonly Type[] Types =
        {
            typeof(ClassWithComparerAsMember_WithHashCodeConfig),
            typeof(ClassWithComparerAsMember_WithComparerAttribute),
            typeof(ClassWithExternComparer_WithHashCodeConfig),
            typeof(ClassWithExternComparer_WithComparerAttribute),
            typeof(ClassWithGenericComparer_WithHashCodeConfig),
            typeof(ClassWithGenericComparer_WithComparerAttribute),
            typeof(ClassWithExternGenericComparer_WithHashCodeConfig),
            typeof(ClassWithExternGenericComparer_WithComparerAttribute),
        };
    }

    internal class ComparerBase<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y) => throw new NotImplementedException();

        public int GetHashCode(T obj) => obj.GetHashCode();
    }

    internal class NullableComparerBase<T> : IEqualityComparer<T>
    {
        public bool Equals(T? x, T? y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(T? obj)
        {
            if (obj is null)
                return 0;
            return obj.GetHashCode();
        }
    }

    public partial class ClassWithComparerAsMember_WithHashCodeConfig
    {
        private class IntComparer : ComparerBase<int> { }

        [EqualityComparer(equalityComparer: typeof(IntComparer))]
        [HashCode]
        public int Property { get; }
    }

    [AutoHashCode]
    public partial class ClassWithComparerAsMember_WithComparerAttribute
    {
        [EqualityComparer(equalityComparer: typeof(IntEqualityComparer))]
        public int Property { get; }
    }

    public partial class ClassWithExternComparer_WithHashCodeConfig
    {
        [EqualityComparer(equalityComparer: typeof(IntEqualityComparer))]
        [HashCode]
        public int Property { get; }
    }

    [AutoHashCode]
    public partial class ClassWithExternComparer_WithComparerAttribute
    {
        [EqualityComparer(equalityComparer: typeof(IntEqualityComparer))]
        public int Property { get; }
    }

    public partial class ClassWithGenericComparer_WithHashCodeConfig
    {
        [EqualityComparer(equalityComparer: typeof(ComparerBase<int>))]
        [HashCode]
        public int Property { get; }
    }

    [AutoHashCode]
    public partial class ClassWithGenericComparer_WithComparerAttribute
    {
        [EqualityComparer(equalityComparer: typeof(ComparerBase<int>))]
        public int Property { get; }
    }

    public partial class ClassWithExternGenericComparer_WithHashCodeConfig
    {
        [EqualityComparer(equalityComparer: typeof(GenericEqualityComparer<int>))]
        [HashCode]
        public int Property { get; }
    }

    [AutoHashCode]
    public partial class ClassWithExternGenericComparer_WithComparerAttribute
    {
        [EqualityComparer(equalityComparer: typeof(GenericEqualityComparer<int>))]
        public int Property { get; }
    }
}

#pragma warning restore
