namespace Aspects.Parsers.CSharp
{
    public enum Variance { In, Out }

    public class GenericTypeArgumentDefinition(string name, Variance? variance, IReadOnlyList<AttributeGroup> attributeGroups)
    {
        public string Name => name;

        public Variance? Variance = variance;

        public IReadOnlyList<AttributeGroup> AttributeGroups => attributeGroups;
    }
}