namespace Aspects.Parsers.CSharp
{
    public class NamespaceInfo
    {
        public NamespaceInfo(string name, IReadOnlyList<UsingDirectiveInfo> directives,
            IReadOnlyList<NamespaceInfo> namespaces, IReadOnlyList<TypeInfo> types, IReadOnlyList<ExternAliasInfo> externAliases)
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

        public NamespaceInfo? ContainingNamespace { get; internal set; }

        public string Name { get; }

        public string FullName() => ContainingNamespace is null or CodeUnit ? Name : $"{ContainingNamespace.FullName()}.{Name}";

        public IReadOnlyList<UsingDirectiveInfo> UsingDirectives { get; }

        public IReadOnlyList<NamespaceInfo> Namespaces { get; }

        public IReadOnlyList<TypeInfo> Types { get; }

        public IReadOnlyList<ExternAliasInfo> ExternAliases { get; }
    }
}
