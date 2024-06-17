namespace Aspects.Parsers.CSharp
{
    public class UsingStaticDirectiveDefinition(string value, string classFullName)
        : UsingDirectiveDefinition(value)
    {
        public string ClassFullName => classFullName;

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Static;
    }
}
