namespace Aspects.Parsers.CSharp
{
    internal class TupleMemberInfo(string type, string? name = null)
    {
        public TupleInfo ContainingType { get; internal set; } = null!;

        public string Type => type;

        public string? Name => name;
    }
}
