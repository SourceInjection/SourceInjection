namespace Aspects.Parsers.CSharp
{
    internal class StructInfo(
        string name, AccessModifier? accessModifier, IReadOnlyList<MemberInfo> members,
        IReadOnlyList<GenericTypeArgumentInfo> genericTypeArguments, IReadOnlyList<ConstraintClauseInfo> constraints,
        bool isRecord, bool isReadonly)

        : StructuredTypeInfo(name, accessModifier, members, genericTypeArguments, constraints)
    {
        public override TypeKind TypeKind { get; } = TypeKind.Struct;

        public bool IsReadonly => isReadonly;

        public bool IsRecord => isRecord;
    }
}
