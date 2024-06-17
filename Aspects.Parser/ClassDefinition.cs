namespace Aspects.Parsers.CSharp
{
    public class ClassDefinition(
        string name, 
        AccessModifier? accessModifier, 
        bool hasNewModifier, 
        IReadOnlyList<AttributeGroup> attributeGroups,
        IReadOnlyList<MemberDefinition> members,
        IReadOnlyList<GenericTypeArgumentDefinition> genericTypeArguments, 
        IReadOnlyList<ConstraintDefinition> constraints,
        bool isRecord, 
        bool isStatic, 
        bool isSealed, 
        bool isAbstract)

        : StructuredTypeDefinition(name, accessModifier, hasNewModifier, attributeGroups, members, genericTypeArguments, constraints)
    {
        public override TypeKind TypeKind { get; } = TypeKind.Class;

        public bool IsStatic => isStatic;

        public bool IsRecord => isRecord;

        public bool IsSealed => isSealed;

        public bool IsAbstract => isAbstract;
    }
}
