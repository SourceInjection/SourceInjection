namespace Aspects.Parsers.CSharp
{
    public class ClassDefinition: StructuredTypeDefinition
    {
        public ClassDefinition(
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

            : base(name, accessModifier, hasNewModifier, attributeGroups, members, genericTypeArguments, constraints)
        {
            IsStatic = isStatic;
            IsRecord = isRecord;
            IsSealed = isSealed;
            IsAbstract = isAbstract;
        }

        public override TypeKind TypeKind { get; } = TypeKind.Class;

        public bool IsStatic { get; }

        public bool IsRecord { get; }

        public bool IsSealed { get; }

        public bool IsAbstract { get; }
    }
}
