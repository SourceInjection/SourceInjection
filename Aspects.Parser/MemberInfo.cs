namespace Aspects.Parsers.CSharp
{
    public enum MemberKind
    {
        Type,
        Property,
        Field,
        Method,
        Event,
    }

    public enum AccessModifier
    {
        Private,
        PrivateProtected,
        Internal,
        Protected,
        ProtectedInternal,
        Public
    }

    public abstract class MemberInfo(string name, AccessModifier? modifier)
    {
        public abstract MemberKind MemberKind { get; }

        public abstract AccessModifier DefaultAccessability { get; }

        public TypeInfo? ContainingType { get; internal set; }

        public string Name => name;

        public AccessModifier? AccessModifier => modifier;

        public bool IsKind(MemberKind kind) => MemberKind == kind;

        public bool HasAccessibility(AccessModifier accessModifier)
        {
            var modifier = AccessModifier ?? DefaultAccessability;
            return accessModifier == modifier;
        }
    }
}
