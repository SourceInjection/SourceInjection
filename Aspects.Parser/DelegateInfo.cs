namespace Aspects.Parsers.CSharp
{
    internal class DelegateInfo(
        string name, AccessModifier accessModifier, string returnType, IReadOnlyList<ParameterInfo> parameters,
        IReadOnlyList<GenericTypeArgumentInfo> genericTypeArguments, IReadOnlyList<ConstraintClauseInfo> constraints) 

        : TypeInfo(name, accessModifier)
    {
        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Internal;

        public override TypeKind TypeKind { get; } = TypeKind.Delegate;

        public string ReturnType => returnType;

        public IReadOnlyList<ParameterInfo> Parameters => parameters;

        public IReadOnlyList<GenericTypeArgumentInfo> GenericTypeArguments => genericTypeArguments;

        public IReadOnlyList<ConstraintClauseInfo> ConstraintClauses => constraints;
    }
}
