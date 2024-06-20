namespace Aspects.Parsers.CSharp.Visitors
{
    internal class InterfaceDefinition: StructuredTypeDefinition
    {
        public InterfaceDefinition(
            string name,
            AccessModifier? accessModifier,
            bool hasNewModifier,
            IReadOnlyList<AttributeGroup> attributeGroups,
            IReadOnlyList<MemberDefinition> members,
            IReadOnlyList<GenericTypeArgumentDefinition> genericTypeArguments,
            IReadOnlyList<ConstraintDefinition> constraints)

            : base(name, accessModifier, hasNewModifier, attributeGroups, members, genericTypeArguments, constraints)
        { }

        public override TypeKind TypeKind { get; } = TypeKind.Interface;
    }
}
