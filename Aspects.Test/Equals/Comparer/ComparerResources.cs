using Aspects.Attributes;
using System.Diagnostics.CodeAnalysis;


#pragma warning disable

namespace Aspects.Test.Equals.Comparer
{
    using NullSafety = Attributes.NullSafety;

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

        public int GetHashCode([DisallowNull] T obj)
        {
            throw new NotImplementedException();
        }
    }

    public partial class ClassWithMember_ThatHasCustomComparer_EqualsConfig
    {
        private class IntComparer : ComparerBase<int> { }

        [Equals(equalityComparer: typeof(IntComparer))]
        public int Property { get; }
    }

    [AutoEquals]
    public partial class ClassWithMember_ThatHasCustomComparer_ComparerAttribute
    {
        private class IntComparer : ComparerBase<int> { }

        [EqualityComparer(equalityComparer: typeof(IntComparer))]
        public int Property { get; }
    }

    public partial class ClassWithNullableNonReferenceTypeMember_ThatHasNonNullableComparer
    {
        private class IntComparer : ComparerBase<int> { }

        [Equals(equalityComparer: typeof(IntComparer))]
        public int? Property { get; set; }
    }


    public partial class ClassWithNullableNonReferenceTypeMember_ThatHasNullableComparer
    {
        private class IntComparer : NullableComparerBase<int?> { }

        [Equals(equalityComparer: typeof(IntComparer))]
        public int? Property { get; set; }
    }

    public partial class ClassWithNullableNonReferenceTypeMember_ThatHasNullableComparer_WithNullsafetyOn
    {
        private class IntComparer : NullableComparerBase<int?> { }

        [Equals(equalityComparer: typeof(IntComparer), nullSafety: NullSafety.On)]
        public int? Property { get; set; }
    }

    public partial class ClassWithNullableNonReferenceTypeMember_ThatHasNonNullableComparer_WithNullsafetyOff
    {
        private class IntComparer : ComparerBase<int> { }

        [Equals(equalityComparer: typeof(IntComparer), nullSafety: NullSafety.Off)]
        public int? Property { get; set; }
    }
}

#pragma warning restore
