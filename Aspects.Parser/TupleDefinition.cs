namespace Aspects.Parsers.CSharp
{
    internal class TupleDefinition : TypeDefinition
    {
        public TupleDefinition(IReadOnlyList<TupleMemberDefinitinon> members)
            : base(string.Empty, CSharp.AccessModifier.Public, false, [])
        {
            Members = members;
            foreach(var member in members)
                member.ContainingType = this;
        }

        public IReadOnlyList<TupleMemberDefinitinon> Members { get; }

        public override TypeKind TypeKind { get; } = TypeKind.Tuple;

        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Public;
    }
}
