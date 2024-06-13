using Aspects.Parsers.CSharp.Tree.Types;
using Aspects.Parsers.CSharp.Tree.Usings;

namespace Aspects.Parsers.CSharp.Tree
{
    public class NamespaceInfo
    {
        public NamespaceInfo(string name, IReadOnlyList<UsingDirective> directives, 
            IReadOnlyList<NamespaceInfo> namespaces, IReadOnlyList<TypeInfo> types)
        {
            Name = name;
            foreach (var ns in namespaces)
                ns.ContainingNamespace = this;
            Namespaces = namespaces;
            foreach (var type in types)
                type.ContainingNamespace = this;
            Types = types;
            foreach(var directive in directives)
                directive.ContainingNamespace = this;
            Directives = directives;
        }

        public NamespaceInfo? ContainingNamespace { get; internal set; }

        public string Name { get; }

        public string FullName() => ContainingNamespace is null ? Name : $"{ContainingNamespace.FullName()}.{Name}";

        public IReadOnlyList<UsingDirective> Directives { get; }

        public IReadOnlyList<NamespaceInfo> Namespaces { get; }

        public IReadOnlyList<TypeInfo> Types { get; }
    }
}
