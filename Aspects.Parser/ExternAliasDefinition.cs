namespace Aspects.Parsers.CSharp
{
    public class ExternAliasDefinition(string name)
    {
        public NamespaceDefinition? ContainingNamespace { get; internal set; }

        public string Name => name;
    }
}
