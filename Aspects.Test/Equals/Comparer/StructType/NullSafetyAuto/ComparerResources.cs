
#pragma warning disable
namespace Aspects.Test.Equals.Comparer.StructType.NullSafetyAuto
{
    internal static class ComparerResources
    {
        public static readonly object[] MustUseComparerEqualization =
        {
            new object[] { typeof(ClassWithNullableMember_WithNullableComparer), false },
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

        [Equals(equalityComparer: typeof(IntComparer))]
        public int? Property { get; set; }
    }

    public partial class ClassWithNullableMember_WithNonNullableComparer
    {
        private class IntComparer : ComparerBase<int> { }

        [Equals(equalityComparer: typeof(IntComparer))]
        public int? Property { get; set; }
    }

    public partial class ClassWithNonNullableMember_WithNullableComparer
    {
        private class IntComparer : NullableComparerBase<int?> { }

        [Equals(equalityComparer: typeof(IntComparer))]
        public int Property { get; set; }
    }

    public partial class ClassWithNonNullableMember_WithNonNullableComparer
    {
        private class IntComparer : ComparerBase<int> { }

        [Equals(equalityComparer: typeof(IntComparer))]
        public int Property { get; set; }
    }
}
#pragma warning restore
