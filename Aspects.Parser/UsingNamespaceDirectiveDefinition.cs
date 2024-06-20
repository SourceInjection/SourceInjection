namespace Aspects.Parsers.CSharp
{
    public class UsingNamespaceDirectiveDefinition: UsingDirectiveDefinition
    {
        public UsingNamespaceDirectiveDefinition(string value, string @namespace)
            : base(value)
        {
            Namespace = @namespace;
        }

        public string Namespace { get; }

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Namespace;
    }
}
