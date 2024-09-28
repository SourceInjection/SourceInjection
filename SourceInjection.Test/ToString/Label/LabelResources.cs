namespace SourceInjection.Test.ToString.Label
{
    internal partial class ClassWithMemberThatHasLabel
    {
        [ToString(label: "Label")]
        public int Property { get; }
    }
}
