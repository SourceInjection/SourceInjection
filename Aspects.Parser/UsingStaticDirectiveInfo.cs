namespace Aspects.Parsers.CSharp
{
    public class UsingStaticDirectiveInfo(string value, string classFullName)
        : UsingDirectiveInfo(value)
    {
        public string ClassFullName => classFullName;

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Static;
    }
}
