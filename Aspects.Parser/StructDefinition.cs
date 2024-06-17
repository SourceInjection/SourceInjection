namespace Aspects.Parsers.CSharp
{
    internal class StructDefinition(
        string name, 
        AccessModifier? accessModifier, 
        bool hasNewModifier, 
        IReadOnlyList<AttributeGroup> attributeGroups,
        IReadOnlyList<MemberDefinition> members,
        IReadOnlyList<GenericTypeArgumentDefinition> genericTypeArguments, 
        IReadOnlyList<ConstraintDefinition> constraints,
        bool isRecord, bool isReadonly)

        : StructuredTypeDefinition(name, accessModifier, hasNewModifier, attributeGroups, members, genericTypeArguments, constraints)
    {
        public override TypeKind TypeKind { get; } = TypeKind.Struct;

        public bool IsReadonly => isReadonly;

        public bool IsRecord => isRecord;
    }
}
