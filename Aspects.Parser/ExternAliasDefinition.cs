namespace Aspects.Parsers.CSharp
{
    public class ExternAliasDefinition
    {
        public ExternAliasDefinition(string name)
        {
            Name = name;
        }

        public NamespaceDefinition? ContainingNamespace { get; internal set; }

        public string Name { get; }
    }
}
