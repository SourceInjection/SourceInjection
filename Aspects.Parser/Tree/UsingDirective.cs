namespace Aspects.Parsers.Tree
{
    public enum UsingDirectiveKind
    {
        Namespace,
        Static,
        Alias
    }

    internal abstract class UsingDirective(string value)
    {
        public string Value => value;

        public abstract UsingDirectiveKind Kind { get; }

        public bool IsKind(UsingDirectiveKind kind) => Kind == kind;

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj == this || obj is UsingDirective other
                && Value == other.Value
                && Kind == other.Kind;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
