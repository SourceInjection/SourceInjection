using Aspects.Parsers.CSharp.Tree.Types;

namespace Aspects.Parsers.CSharp.Tree.Members
{
    internal class EnumMemberInfo(string name, string? value)
        : MemberInfo(name, AccessModifier.Public)
    {
        public new EnumInfo ContainingType => (EnumInfo)base.ContainingType!;

        public string? Value => value;

        public override MemberKind MemberKind { get; } = MemberKind.EnumMember;
    }
}
