namespace Aspects.Parsers.CSharp
{
    public class PropertyInfo(string name, string type, AccessModifier accessModifier,
        bool isAbstract, bool isVirtual, bool isOverride, bool isNew,
        string? getter = null, string? setter = null, string? initialization = null)

        : MemberInfo(name, accessModifier)
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

        public bool IsNew => isNew;
    }
}
