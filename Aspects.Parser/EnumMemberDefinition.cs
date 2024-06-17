namespace Aspects.Parsers.CSharp
{
    internal class EnumMemberDefinition(string name, string? value)
    {
        public EnumDefinition ContainingType { get; internal set; } = null!;

        public string Name => name;

        public string? Value => value;

        public string FullName() => $"{ContainingType.FullName()}.{Name}";
    }
}
