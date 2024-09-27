namespace Aspects.Test.ToString.Format
{
    internal partial class ClassWithCustomFormat
    {
        [ToString(format: "HH:mm")]
        public DateTime Property { get; }
    }

    internal partial class ClassWithCustomFormatAtNullableMember
    {
        [ToString(format: "HH:mm")]
        public DateTime? Property { get; }
    }
}
