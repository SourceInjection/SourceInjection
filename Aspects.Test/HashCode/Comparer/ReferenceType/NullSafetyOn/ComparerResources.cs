
#pragma warning disable
namespace Aspects.Test.HashCode.Comparer.ReferenceType.NullSafetyOn
{
    using NullSafety = Aspects.NullSafety;

    internal static class ComparerResources
    {
        public static readonly object[] Types =
        {
            new object[] { typeof(ClassWithNullableMember_WithNullableComparer), true },
            new object[] { typeof(ClassWithNullableMember_WithNonNullableComparer), true },
            new object[] { typeof(ClassWithNonNullableMember_WithNullableComparer), true },
            new object[] { typeof(ClassWithNonNullableMember_WithNonNullableComparer), true },
        };
    }

    public partial class ClassWithNullableMember_WithNullableComparer
    {
        private class IntComparer : NullableComparerBase<object?> { }

        [HashCode(equalityComparer: typeof(IntComparer), nullSafety: NullSafety.On)]
        public object? Property { get; set; }
    }

    public partial class ClassWithNullableMember_WithNonNullableComparer
    {
        private class IntComparer : ComparerBase<object> { }

        [HashCode(equalityComparer: typeof(IntComparer), nullSafety: NullSafety.On)]
        public object? Property { get; set; }
    }

    public partial class ClassWithNonNullableMember_WithNullableComparer
    {
        private class IntComparer : NullableComparerBase<object?> { }

        [HashCode(equalityComparer: typeof(IntComparer), nullSafety: NullSafety.On)]
        public object Property { get; set; }
    }

    public partial class ClassWithNonNullableMember_WithNonNullableComparer
    {
        private class IntComparer : ComparerBase<object> { }

        [HashCode(equalityComparer: typeof(IntComparer), nullSafety: NullSafety.On)]
        public object Property { get; set; }
    }
}
#pragma warning restore
