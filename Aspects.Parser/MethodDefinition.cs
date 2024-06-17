namespace Aspects.Parsers.CSharp
{
    public class MethodDefinition(string name, AccessModifier accessModifier, bool hasNewModifier,
        IReadOnlyList<string> genericTypeArguments, IReadOnlyList<ParameterDefinition> parameters,
        string returnType, string body, bool isAbstract, bool isVirtual, bool isOverride, bool isNew)

        : MemberDefinition(name, accessModifier, hasNewModifier)
    {
        public override MemberKind MemberKind { get; } = MemberKind.Method;

        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Private;

        public string Body => body;

        public string ReturnType => returnType;

        public IReadOnlyList<ParameterDefinition> Parameters => parameters;

        public IReadOnlyList<string> GenericTypeArguments => genericTypeArguments;

        public bool IsAbstract => isAbstract;

        public bool IsVirtual => isVirtual;

        public bool IsOverride => isOverride;

        public bool IsNew => isNew;
    }
}
