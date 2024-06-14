namespace Aspects.Parsers.CSharp
{
    public class UsingNamespaceDirectiveInfo(string value, string nameSpace)
        : UsingDirectiveInfo(value)
    {
        public string Namespace => nameSpace;

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Namespace;
    }
}
