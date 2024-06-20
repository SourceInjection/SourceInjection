namespace Aspects.Parsers.CSharp
{
    public class EnumDefinition : TypeDefinition
    {
        public EnumDefinition(
            string name, 
            AccessModifier? accessModifier, 
            bool hasNewModifier, 
            IReadOnlyList<AttributeGroup> attributeGroups,
            IReadOnlyList<EnumMemberDefinition> members, 
            string? intType)
            
            : base(name, accessModifier, hasNewModifier, attributeGroups)
        {
            foreach (var member in members)
                member.ContainingType = this;

            Members = members;
            IntType = intType;
        }

        public IReadOnlyList<EnumMemberDefinition> Members { get; }

        public override TypeKind TypeKind { get; } = TypeKind.Enum;

        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Internal;

        public string? IntType { get; }
    }
}
