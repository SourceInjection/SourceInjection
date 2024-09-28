#pragma warning disable

namespace SourceInjection.Test.HashCode.Comparer
{
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

        [HashCode(equalityComparer: typeof(IntComparer))]
        public int Property { get; }
    }

    [AutoHashCode]
    public partial class ClassWithComparerAsMember_WithComparerAttribute
    {
        [EqualityComparer(equalityComparer: typeof(Comparers.IntEqualityComparer))]
        public int Property { get; }
    }

    public partial class ClassWithExternComparer_WithHashCodeConfig
    {
        [HashCode(equalityComparer: typeof(Comparers.IntEqualityComparer))]
        public int Property { get; }
    }

    [AutoHashCode]
    public partial class ClassWithExternComparer_WithComparerAttribute
    {
        [EqualityComparer(equalityComparer: typeof(Comparers.IntEqualityComparer))]
        public int Property { get; }
    }

    public partial class ClassWithGenericComparer_WithHashCodeConfig
    {
        [HashCode(equalityComparer: typeof(ComparerBase<int>))]
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
        [HashCode(equalityComparer: typeof(Comparers.GenericEqualityComparer<int>))]
        public int Property { get; }
    }

    [AutoHashCode]
    public partial class ClassWithExternGenericComparer_WithComparerAttribute
    {
        [EqualityComparer(equalityComparer: typeof(Comparers.GenericEqualityComparer<int>))]
        public int Property { get; }
    }
}

#pragma warning restore
