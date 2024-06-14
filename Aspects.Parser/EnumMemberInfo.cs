namespace Aspects.Parsers.CSharp
{
    internal class EnumMemberInfo(string name, string? value)
    {
        public EnumInfo ContainingType { get; internal set; } = null!;

        public string Name => name;

        public string? Value => value;

        public string FullName() => $"{ContainingType.FullName()}.{Name}";
    }
}
