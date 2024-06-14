namespace Aspects.Parsers.CSharp
{
    internal class EnumMemberInfo(string name, string? value)
    {
        public EnumInfo? ContainingType { get; internal set; }

        public string Name => name;

        public string? Value => value;

        public string FullName()
        {
            return ContainingType is null ? Name
                : $"{ContainingType.FullName()}.{Name}";
        }
    }
}
