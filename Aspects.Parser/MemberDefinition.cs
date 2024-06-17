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

    public abstract class MemberDefinition(string name, AccessModifier? modifier, bool hasNewModifier)
    {
        public abstract MemberKind MemberKind { get; }

        public abstract AccessModifier DefaultAccessability { get; }

        public TypeDefinition? ContainingType { get; internal set; }

        public string Name => name;

        public bool HasNewModifer => hasNewModifier;

        public AccessModifier? AccessModifier => modifier;

        public bool IsKind(MemberKind kind) => MemberKind == kind;

        public bool HasAccessibility(AccessModifier accessModifier)
        {
            var modifier = AccessModifier ?? DefaultAccessability;
            return accessModifier == modifier;
        }
    }
}
