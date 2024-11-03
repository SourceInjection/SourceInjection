
#pragma warning disable
namespace SourceInjection.Test.HashCode.Comparer.ReferenceType.NullSafetyOff
{
    using NullSafety = SourceInjection.NullSafety;

    internal static class ComparerResources
    {
        public static readonly object[] Types =
        {
            new object[] { typeof(ClassWithNullableMember_WithNullableComparer), false },
            new object[] { typeof(ClassWithNullableMember_WithNonNullableComparer), false },
            new object[] { typeof(ClassWithNonNullableMember_WithNullableComparer), false },
            new object[] { typeof(ClassWithNonNullableMember_WithNonNullableComparer), false },
        };
    }

    public partial class ClassWithNullableMember_WithNullableComparer
    {
        private class IntComparer : NullableComparerBase<object?> { }

        [EqualityComparer(equalityComparer: typeof(IntComparer), nullSafety: NullSafety.Off)]
        [HashCode]
        public object? Property { get; set; }
    }

    public partial class ClassWithNullableMember_WithNonNullableComparer
    {
        private class IntComparer : ComparerBase<object> { }

        [EqualityComparer(equalityComparer: typeof(IntComparer), nullSafety: NullSafety.Off)]
        [HashCode]
        public object? Property { get; set; }
    }

    public partial class ClassWithNonNullableMember_WithNullableComparer
    {
        private class IntComparer : NullableComparerBase<object?> { }

        [EqualityComparer(equalityComparer: typeof(IntComparer), nullSafety: NullSafety.Off)]
        [HashCode]
        public object Property { get; set; }
    }

    public partial class ClassWithNonNullableMember_WithNonNullableComparer
    {
        private class IntComparer : ComparerBase<object> { }

        [EqualityComparer(equalityComparer: typeof(IntComparer), nullSafety: NullSafety.Off)]
        [HashCode]
        public object Property { get; set; }
    }
}
#pragma warning restore
