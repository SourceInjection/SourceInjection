namespace Aspects.Parsers.CSharp
{
    public abstract class StructuredTypeDefinition : TypeDefinition
    {
        protected StructuredTypeDefinition(
            string name, AccessModifier? accessModifier, bool hasNewModifier, IReadOnlyList<AttributeGroup> attributeGroups,
            IReadOnlyList<MemberDefinition> members, 
            IReadOnlyList<GenericTypeArgumentDefinition> genericTypeArguments, 
            IReadOnlyList<ConstraintDefinition> constraints)
            
            : base(name, accessModifier, hasNewModifier, attributeGroups)
        {
            foreach(var member in members)
                member.ContainingType = this;

            GenericTypeArguments = genericTypeArguments;
            ConstraintClauses = constraints;
            Members = members;
            Fields = members.OfType<FieldDefinition>().ToArray();
            Properties = members.OfType<PropertyDefinition>().ToArray();
            Methods = members.OfType<MethodDefinition>().ToArray();
            Types = members.OfType<TypeDefinition>().ToArray();
        }

        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Internal;

        public IReadOnlyList<MemberDefinition> Members { get; }

        public IReadOnlyList<FieldDefinition> Fields { get; }

        public IReadOnlyList<PropertyDefinition> Properties { get; }

        public IReadOnlyList<MethodDefinition> Methods { get; }

        public IReadOnlyList<TypeDefinition> Types { get; }

        public IReadOnlyList<GenericTypeArgumentDefinition> GenericTypeArguments { get; }

        public IReadOnlyList<ConstraintDefinition> ConstraintClauses { get; }
    }
}
