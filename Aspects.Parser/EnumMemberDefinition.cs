namespace Aspects.Parsers.CSharp
{
    internal class EnumMemberDefinition(string name, string? value, IReadOnlyList<AttributeGroup> attributeGroups)
    {
        public EnumDefinition ContainingType { get; internal set; } = null!;

        public string Name => name;

        public string? Value => value;

        public IReadOnlyList<AttributeGroup> AttributeGroups => attributeGroups;

        public string FullName() => $"{ContainingType.FullName()}.{Name}";
    }
}
