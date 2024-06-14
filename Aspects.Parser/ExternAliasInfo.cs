namespace Aspects.Parsers.CSharp
{
    public class ExternAliasInfo(string name)
    {
        public NamespaceInfo? ContainingNamespace { get; internal set; }

        public string Name => name;
    }
}
