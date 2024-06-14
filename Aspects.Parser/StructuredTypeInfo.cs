namespace Aspects.Parsers.CSharp
{
    public abstract class StructuredTypeInfo : TypeInfo
    {
        protected StructuredTypeInfo(string name, AccessModifier? accessModifier, IReadOnlyList<MemberInfo> members, 
            IReadOnlyList<GenericTypeArgumentInfo> genericTypeArguments, IReadOnlyList<ConstraintClauseInfo> constraints)
            : base(name, accessModifier)
        {
            foreach(var member in members)
                member.ContainingType = this;

            GenericTypeArguments = genericTypeArguments;
            ConstraintClauses = constraints;
            Members = members;
            Fields = members.OfType<FieldInfo>().ToArray();
            Properties = members.OfType<PropertyInfo>().ToArray();
            Methods = members.OfType<MethodInfo>().ToArray();
            Types = members.OfType<TypeInfo>().ToArray();
        }

        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Internal;

        public IReadOnlyList<MemberInfo> Members { get; }

        public IReadOnlyList<FieldInfo> Fields { get; }

        public IReadOnlyList<PropertyInfo> Properties { get; }

        public IReadOnlyList<MethodInfo> Methods { get; }

        public IReadOnlyList<TypeInfo> Types { get; }

        public IReadOnlyList<GenericTypeArgumentInfo> GenericTypeArguments { get; }

        public IReadOnlyList<ConstraintClauseInfo> ConstraintClauses { get; }
    }
}
