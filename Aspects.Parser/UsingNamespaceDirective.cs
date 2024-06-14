namespace Aspects.Parsers.CSharp
{
    public class UsingNamespaceDirective(string value, string nameSpace)
        : UsingDirective(value)
    {
        public string Namespace => nameSpace;

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Namespace;
    }
}
