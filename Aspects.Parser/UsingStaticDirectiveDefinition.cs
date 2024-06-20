namespace Aspects.Parsers.CSharp
{
    public class UsingStaticDirectiveDefinition: UsingDirectiveDefinition
    {
        public UsingStaticDirectiveDefinition(string value, string classFullName)
            : base(value)
        {
            ClassFullName = classFullName;
        }

        public string ClassFullName { get; }

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Static;
    }
}
