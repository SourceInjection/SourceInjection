namespace Aspects.Parsers.CSharp
{
    public enum TypeKind { Enum, Class, Record, Struct, Interface, Delegate }

    public abstract class TypeInfo(string name, AccessModifier? accessModifier) : MemberInfo(name, accessModifier)
    {
        public abstract TypeKind TypeKind { get; }

        public override MemberKind MemberKind { get; } = MemberKind.Type;

        public NamespaceInfo? ContainingNamespace { get; internal set; }

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
