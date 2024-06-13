using Aspects.Parsers.CSharp.Tree.Members;

namespace Aspects.Parsers.CSharp.Tree.Types
{
    public enum TypeKind { Enum, Class, Record, Struct }

    public abstract class TypeInfo : MemberInfo
    {
        protected TypeInfo(string name, AccessModifier accessModifier, IReadOnlyList<MemberInfo> members)
            : base(name, accessModifier)
        {
            Members = members;
            foreach (var member in members)
                member.ContainingType = this;
        }

        public NamespaceInfo? ContainingNamespace { get; internal set; }

        public string FullName()
        {
            if (ContainingNamespace is not null)
                return $"{ContainingNamespace.FullName()}.{Name}";
            if (ContainingType is not null)
                return $"{ContainingType.FullName()}.{Name}";
            return Name;
        }

        public abstract TypeKind TypeKind { get; }

        public override MemberKind MemberKind { get; } = MemberKind.Type;

        public IReadOnlyList<MemberInfo> Members { get; }
    }
}
