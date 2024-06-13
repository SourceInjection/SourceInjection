namespace Aspects.Parsers.CSharp.Tree
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    internal class TypeInfo(TypeInfo? containingType, string name, string nameSpace, IReadOnlyList<MemberInfo> members)
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        : MemberInfo(containingType, name)
    {
        public string NameSpace => nameSpace;

        public string FullName { get; } = $"{nameSpace}.{name}";

        public override MemberKind Kind { get; } = MemberKind.Type;

        public IReadOnlyList<MemberInfo> Members => members;

        public override bool Equals(object? obj)
        {
            return obj == this || obj is TypeInfo other
                && base.Equals(obj)
                && other.NameSpace == NameSpace
                && other.Members.SequenceEqual(Members);
        }
    }
}
