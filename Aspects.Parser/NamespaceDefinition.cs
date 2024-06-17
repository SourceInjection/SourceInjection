namespace Aspects.Parsers.CSharp
{
    public class NamespaceDefinition
    {
        public NamespaceDefinition(string name, IReadOnlyList<UsingDirectiveDefinition> directives,
            IReadOnlyList<NamespaceDefinition> namespaces, IReadOnlyList<TypeDefinition> types, IReadOnlyList<ExternAliasDefinition> externAliases)
        {
            Name = name;
            foreach (var ns in namespaces)
                ns.ContainingNamespace = this;
            Namespaces = namespaces;
            foreach (var type in types)
                type.ContainingNamespace = this;
            Types = types;
            foreach (var directive in directives)
                directive.ContainingNamespace = this;
            UsingDirectives = directives;
            foreach (var alias in externAliases)
                alias.ContainingNamespace = this;
            ExternAliases = externAliases;
        }

        public NamespaceDefinition? ContainingNamespace { get; internal set; }

        public string Name { get; }

        public string FullName() => ContainingNamespace is null or CodeUnit ? Name : $"{ContainingNamespace.FullName()}.{Name}";

        public IReadOnlyList<UsingDirectiveDefinition> UsingDirectives { get; }

        public IReadOnlyList<NamespaceDefinition> Namespaces { get; }

        public IReadOnlyList<TypeDefinition> Types { get; }

        public IReadOnlyList<ExternAliasDefinition> ExternAliases { get; }
    }
}
