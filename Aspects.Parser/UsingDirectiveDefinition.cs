namespace Aspects.Parsers.CSharp
{
    public enum UsingDirectiveKind
    {
        Namespace,
        Static,
        Alias,
        TupleDefinition
    }

    public abstract class UsingDirectiveDefinition(string value)
    {
        public NamespaceDefinition? ContainingNamespace { get; internal set; }

        public string Value => value;

        public abstract UsingDirectiveKind Kind { get; }

        public bool IsKind(UsingDirectiveKind kind) => Kind == kind;
    }
}
