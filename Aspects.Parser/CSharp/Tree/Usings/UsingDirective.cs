namespace Aspects.Parsers.CSharp.Tree.Usings
{
    public enum UsingDirectiveKind
    {
        Namespace,
        Static,
        Alias
    }

    public abstract class UsingDirective(string value)
    {
        public NamespaceInfo? ContainingNamespace { get; internal set; }

        public string Value => value;

        public abstract UsingDirectiveKind Kind { get; }

        public bool IsKind(UsingDirectiveKind kind) => Kind == kind;
    }
}
