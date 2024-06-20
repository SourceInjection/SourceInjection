namespace Aspects.Parsers.CSharp
{
    public enum UsingDirectiveKind
    {
        Namespace,
        Static,
        Alias,
        TupleDefinition
    }

    public abstract class UsingDirectiveDefinition
    {
        public UsingDirectiveDefinition(string value)
        {
            Value = value;
        }

        public NamespaceDefinition? ContainingNamespace { get; internal set; }

        public string Value { get; }

        public abstract UsingDirectiveKind Kind { get; }

        public bool IsKind(UsingDirectiveKind kind) => Kind == kind;
    }
}
