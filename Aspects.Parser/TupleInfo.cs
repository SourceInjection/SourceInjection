namespace Aspects.Parsers.CSharp
{
    internal class TupleInfo : TypeInfo
    {
        public TupleInfo(IReadOnlyList<TupleMemberInfo> members)
            : base(string.Empty, CSharp.AccessModifier.Public)
        {
            Members = members;
            foreach(var member in members)
                member.ContainingType = this;
        }

        public IReadOnlyList<TupleMemberInfo> Members { get; }

        public override TypeKind TypeKind { get; } = TypeKind.Tuple;

        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Public;
    }
}
