namespace Aspects.Parsers.CSharp
{
    public class ClassInfo(
        string name, AccessModifier? accessModifier, IReadOnlyList<MemberInfo> members,
        IReadOnlyList<GenericTypeArgumentInfo> genericTypeArguments, IReadOnlyList<ConstraintClauseInfo> constraints,
        bool isRecord, bool isStatic, bool isSealed)

        : StructuredTypeInfo(name, accessModifier, members, genericTypeArguments, constraints)
    {
        public override TypeKind TypeKind { get; } = TypeKind.Class;

        public bool IsStatic => isStatic;

        public bool IsRecord => isRecord;

        public bool IsSealed => isSealed;
    }
}
