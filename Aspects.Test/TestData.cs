using Aspects.Attributes;

namespace Aspects.Test
{
    [HashCode]
    [Equals]
    [ToString(DataMemberKind.Public, nameof(Value), nameof(Int))]
    internal partial class TestData<T>
    {
        private const int constInt = 3;
        private int _int;

        public int Int => _int;

        [HashCodeExclude]
        [EqualsExclude]
        public int NotRelevant => constInt;

        public string Value { get; set; }
    }
}
