namespace Aspects.Parsers.CSharp
{
    public class EnumMemberDefinition
    {
        public EnumMemberDefinition(string name, string? value, IReadOnlyList<AttributeGroup> attributeGroups)
        {
            Name = name;
            Value = value;
            AttributeGroups = attributeGroups;
        }

        public EnumDefinition ContainingType { get; internal set; } = null!;

        public string Name { get; }

        public string? Value { get; }

        public IReadOnlyList<AttributeGroup> AttributeGroups { get; }

        public string FullName() => $"{ContainingType.FullName()}.{Name}";
    }
}
