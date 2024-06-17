namespace Aspects.Parsers.CSharp
{
    public class UsingNamespaceDirectiveDefinition(string value, string nameSpace)
        : UsingDirectiveDefinition(value)
    {
        public string Namespace => nameSpace;

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Namespace;
    }
}
