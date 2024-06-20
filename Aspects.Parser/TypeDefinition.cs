namespace Aspects.Parsers.CSharp
{
    public enum TypeKind 
    {
        Enum, 
        Class, 
        Record, 
        Struct,
        Interface, 
        Delegate, 
        Tuple 
    }

    public abstract class TypeDefinition: MemberDefinition
    {
        public TypeDefinition(
            string name, 
            AccessModifier? accessModifier, 
            bool hasNewModifier, 
            IReadOnlyList<AttributeGroup> attributeGroups)

            : base(name, accessModifier, hasNewModifier)
        {
            AttributeGroups = attributeGroups;
            Attributes = attributeGroups.SelectMany(g => g.Attributes).ToArray();
        }

        public abstract TypeKind TypeKind { get; }

        public override MemberKind MemberKind { get; } = MemberKind.Type;

        public NamespaceDefinition? ContainingNamespace { get; internal set; }

        public IReadOnlyList<AttributeGroup> AttributeGroups { get; }

        public IReadOnlyList<AttributeUsage> Attributes { get; }

        public string FullName()
        {
            if (ContainingNamespace is not null)
                return $"{ContainingNamespace.FullName()}.{Name}";
            if (ContainingType is not null)
                return $"{ContainingType.FullName()}.{Name}";
            return Name;
        }
    }
}
