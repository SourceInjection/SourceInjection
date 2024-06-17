namespace Aspects.Parsers.CSharp
{
    public class PropertyDefinition(string name, AccessModifier accessModifier, bool hasNewModifier,
        string type, bool isAbstract, bool isVirtual, bool isOverride,
        string? getter = null, string? setter = null, string? initialization = null)

        : MemberDefinition(name, accessModifier, hasNewModifier)
    {
        public override MemberKind MemberKind { get; } = MemberKind.Property;

        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Private;

        public string Type => type;

        public string? Getter => getter;

        public string? Setter => setter;

        public string? Initialization => initialization;

        public bool IsAbstract => isAbstract;

        public bool IsVirtual => isVirtual;

        public bool IsOverride => isOverride;
    }
}
