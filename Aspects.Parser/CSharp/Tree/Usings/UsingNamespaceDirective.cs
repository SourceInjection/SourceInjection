namespace Aspects.Parsers.CSharp.Tree.Usings
{
    public class UsingNamespaceDirective(string value, string nameSpace)
        : UsingDirective(value)
    {
        public string Namespace => nameSpace;

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Namespace;
    }
}
