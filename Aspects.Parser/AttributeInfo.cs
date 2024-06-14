namespace Aspects.Parsers.CSharp
{
    public class AttributeInfo(string name, IReadOnlyList<ArgumentInfo> arguments)
    {
        public AttributeGroupInfo ContainingSection { get; internal set; } = null!;

        public string Name => name;

        public IReadOnlyList<ArgumentInfo> Arguments => arguments;
    }
}
