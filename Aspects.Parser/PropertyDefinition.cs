namespace Aspects.Parsers.CSharp
{
    public class PropertyDefinition: MemberDefinition
    {
        public PropertyDefinition(
            string name, AccessModifier accessModifier, bool hasNewModifier,
            string type, bool isAbstract, bool isVirtual, bool isOverride)

            : base(name, accessModifier, hasNewModifier)
        {
            Type = type;
            IsAbstract = isAbstract;
            IsVirtual = isVirtual;
            IsOverride = isOverride;
        }

        public override MemberKind MemberKind { get; } = MemberKind.Property;

        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Private;

        public string Type { get; }

        public bool IsAbstract { get; }

        public bool IsVirtual { get; }

        public bool IsOverride { get; }
    }
}
