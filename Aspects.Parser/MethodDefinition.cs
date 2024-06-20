namespace Aspects.Parsers.CSharp
{
    public class MethodDefinition: MemberDefinition
    {
        public MethodDefinition(string name, AccessModifier accessModifier, bool hasNewModifier,
            IReadOnlyList<string> genericTypeArguments, IReadOnlyList<ParameterDefinition> parameters,
            string returnType, bool isAbstract, bool isVirtual, bool isOverride, bool isNew, bool isSealed)

            : base(name, accessModifier, hasNewModifier)
        {
            GenericTypeArguments = genericTypeArguments;
            Parameters = parameters;
            ReturnType = returnType;
            IsAbstract = isAbstract;
            IsVirtual = isVirtual;
            IsOverride = isOverride;
            IsNew = isNew;
            IsSealed = isSealed;
        }

        public override MemberKind MemberKind { get; } = MemberKind.Method;

        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Private;

        public string ReturnType { get; }

        public IReadOnlyList<ParameterDefinition> Parameters { get; }

        public IReadOnlyList<string> GenericTypeArguments { get; }

        public bool IsAbstract { get; }

        public bool IsVirtual { get; }

        public bool IsOverride { get; }

        public bool IsNew { get; }

        public bool IsSealed { get; }
    }
}
