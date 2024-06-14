namespace Aspects.Parsers.CSharp.Visitors
{
    internal class InterfaceInfo(string name, AccessModifier? accessModifier, IReadOnlyList<MemberInfo> members,
        IReadOnlyList<GenericTypeArgumentInfo> genericTypeArguments, IReadOnlyList<ConstraintClauseInfo> constraints)

        : StructuredTypeInfo(name, accessModifier, members, genericTypeArguments, constraints)
    {
        public override TypeKind TypeKind { get; } = TypeKind.Interface;
    }
}
