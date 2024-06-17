namespace Aspects.Parsers.CSharp
{
    public class AttributeUsage(string name, IReadOnlyList<Argument> arguments)
    {
        public AttributeGroup ContainingSection { get; internal set; } = null!;

        public string Name => name;

        public IReadOnlyList<Argument> Arguments => arguments;
    }
}
