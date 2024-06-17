namespace Aspects.Parsers.CSharp.Visitors
{
    internal class InterfaceDefinition(
        string name, 
        AccessModifier? accessModifier, 
        bool hasNewModifier, 
        IReadOnlyList<AttributeGroup> attributeGroups,
        IReadOnlyList<MemberDefinition> members,
        IReadOnlyList<GenericTypeArgumentDefinition> genericTypeArguments, 
        IReadOnlyList<ConstraintDefinition> constraints)

        : StructuredTypeDefinition(name, accessModifier, hasNewModifier, attributeGroups, members, genericTypeArguments, constraints)
    {
        public override TypeKind TypeKind { get; } = TypeKind.Interface;
    }
}
