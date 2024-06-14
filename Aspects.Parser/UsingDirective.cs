namespace Aspects.Parsers.CSharp
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
