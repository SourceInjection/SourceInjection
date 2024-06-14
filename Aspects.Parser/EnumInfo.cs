namespace Aspects.Parsers.CSharp
{
    internal class EnumInfo : TypeInfo
    {
        public EnumInfo(string name, AccessModifier? accessModifier, IReadOnlyList<EnumMemberInfo> members, string? intType = null)
            : base(name, accessModifier)
        {
            foreach (var member in members)
                member.ContainingType = this;

            Members = members;
            IntType = intType;
        }

        public IReadOnlyList<EnumMemberInfo> Members { get; }

        public override TypeKind TypeKind { get; } = TypeKind.Enum;

        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Internal;

        public string? IntType { get; }
    }
}
