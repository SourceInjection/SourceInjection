namespace Aspects.Parsers.CSharp
{
    internal class TupleMemberDefinitinon(string type, string? name = null)
    {
        public TupleDefinition ContainingType { get; internal set; } = null!;

        public string Type => type;

        public string? Name => name;
    }
}
