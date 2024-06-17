namespace Aspects.Parsers.CSharp
{
    internal class DelegateDefinition(
        string name, AccessModifier? accessModifier, bool hasNewModifier, IReadOnlyList<AttributeGroup> attributeGroups,
        string returnType, IReadOnlyList<ParameterDefinition> parameters,
        IReadOnlyList<GenericTypeArgumentDefinition> genericTypeArguments, IReadOnlyList<ConstraintDefinition> constraints) 

        : TypeDefinition(name, accessModifier, hasNewModifier, attributeGroups)
    {
        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Internal;

        public override TypeKind TypeKind { get; } = TypeKind.Delegate;

        public string ReturnType => returnType;

        public IReadOnlyList<ParameterDefinition> Parameters => parameters;

        public IReadOnlyList<GenericTypeArgumentDefinition> GenericTypeArguments => genericTypeArguments;

        public IReadOnlyList<ConstraintDefinition> ConstraintClauses => constraints;
    }
}
