
#pragma warning disable
namespace SourceInjection.Test.Equals.Comparer.StructType.NullSafetyOn
{
    using NullSafety = SourceInjection.NullSafety;

    internal static class ComparerResources
    {
        public static readonly object[] MustUseComparerEqualization =
        {
            new object[] { typeof(ClassWithNullableMember_WithNullableComparer), true },
            new object[] { typeof(ClassWithNonNullableMember_WithNullableComparer), false },
            new object[] { typeof(ClassWithNonNullableMember_WithNonNullableComparer), false },
        };

        public static readonly object[] MustUseComparerStructTypeEqualization =
        {
            new object[] { typeof(ClassWithNullableMember_WithNonNullableComparer), true },
        };
    }

    public partial class ClassWithNullableMember_WithNullableComparer
    {
        private class IntComparer : NullableComparerBase<int?> { }

        [EqualityComparer(equalityComparer: typeof(IntComparer), nullSafety: NullSafety.On)]
        [Equals]
        public int? Property { get; set; }
    }

    public partial class ClassWithNullableMember_WithNonNullableComparer
    {
        private class IntComparer : ComparerBase<int> { }

        [EqualityComparer(equalityComparer: typeof(IntComparer), nullSafety: NullSafety.On)]
        [Equals]
        public int? Property { get; set; }
    }

    public partial class ClassWithNonNullableMember_WithNullableComparer
    {
        private class IntComparer : NullableComparerBase<int?> { }

        [EqualityComparer(equalityComparer: typeof(IntComparer), nullSafety: NullSafety.On)]
        [Equals]
        public int Property { get; set; }
    }

    public partial class ClassWithNonNullableMember_WithNonNullableComparer
    {
        private class IntComparer : ComparerBase<int> { }

        [EqualityComparer(equalityComparer: typeof(IntComparer), nullSafety: NullSafety.On)]
        [Equals]
        public int Property { get; set; }
    }
}
#pragma warning restore
