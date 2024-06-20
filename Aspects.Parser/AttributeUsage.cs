namespace Aspects.Parsers.CSharp
{
    public class AttributeUsage
    {
        public AttributeUsage(string name, IReadOnlyList<Argument> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public AttributeGroup ContainingSection { get; internal set; } = null!;

        public string Name { get; }

        public IReadOnlyList<Argument> Arguments { get; }
    }
}
