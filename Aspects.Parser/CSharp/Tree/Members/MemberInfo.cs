using Aspects.Parsers.CSharp.Tree.Types;

namespace Aspects.Parsers.CSharp.Tree.Members
{
    public enum MemberKind
    {
        Type,
        Property,
        Field,
        Method,
        Event,
        EnumMember
    }

    public abstract class MemberInfo(string name, AccessModifier modifier)
    {
        public TypeInfo? ContainingType { get; internal set; }

        public string Name => name;

        public AccessModifier AccessModifier => modifier;

        public abstract MemberKind MemberKind { get; }

        public bool IsKind(MemberKind kind) => MemberKind == kind;

        public bool HasAccessibility(AccessModifier accessModifier) => AccessModifier == accessModifier;
    }
}
