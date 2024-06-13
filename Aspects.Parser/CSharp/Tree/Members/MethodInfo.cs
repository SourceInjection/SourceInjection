namespace Aspects.Parsers.CSharp.Tree.Members
{
    public class MethodInfo(string name, AccessModifier accessModifier, 
        IReadOnlyList<string> genericTypeArguments, IReadOnlyList<ParameterInfo> parameters,
        string returnType, string body, bool isAbstract, bool isVirtual, bool isOverride, bool isNew)

        : MemberInfo(name, accessModifier)
    {
        public string Body => body;

        public string ReturnType => returnType;

        public IReadOnlyList<ParameterInfo> Parameters => parameters;

        public IReadOnlyList<string> GenericTypeArguments => genericTypeArguments;

        public bool IsAbstract => isAbstract;

        public bool IsVirtual => isVirtual;

        public bool IsOverride => isOverride;

        public bool IsNew => isNew;

        public override MemberKind MemberKind { get; } = MemberKind.Method;
    }
}
