namespace Aspects.Parsers.CSharp
{
    internal class DelegateDefinition : TypeDefinition
    {
        public DelegateDefinition(
            string name, AccessModifier? accessModifier, bool hasNewModifier, IReadOnlyList<AttributeGroup> attributeGroups,
            string returnType, IReadOnlyList<ParameterDefinition> parameters,
            IReadOnlyList<GenericTypeArgumentDefinition> genericTypeArguments, IReadOnlyList<ConstraintDefinition> constraints)

            : base(name, accessModifier, hasNewModifier, attributeGroups)
        {
            ReturnType = returnType;
            Parameters = parameters;
            GenericTypeArguments = genericTypeArguments;
            Constraints = constraints;
        }

        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Internal;

        public override TypeKind TypeKind { get; } = TypeKind.Delegate;

        public string ReturnType { get; }

        public IReadOnlyList<ParameterDefinition> Parameters { get; }

        public IReadOnlyList<GenericTypeArgumentDefinition> GenericTypeArguments { get; }

        public IReadOnlyList<ConstraintDefinition> Constraints { get; }
    }
}
