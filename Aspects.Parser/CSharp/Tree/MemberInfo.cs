namespace Aspects.Parsers.CSharp.Tree
{
    public enum MemberKind
    {
        Type,
        Property,
        Field,
        Method,
        Event,
    }

    internal abstract class MemberInfo(TypeInfo? containingType, string name)
    {
        public TypeInfo? ContainingType => containingType;

        public string Name => name;

        public abstract MemberKind Kind { get; }

        public bool IsKind(MemberKind kind) => Kind == kind;

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj == this || obj is MemberInfo other
                && other.Name == Name
                && other.Kind == Kind;
        }
    }
}
