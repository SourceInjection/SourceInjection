using Antlr4.Runtime;
using Aspects.Parsers.CSharp.Generated;
using Aspects.Parsers.CSharp.Listeners;
using Aspects.Parsers.CSharp.Visitors;

namespace Aspects.Parsers.CSharp
{
    public class CodeUnit: NamespaceInfo
    {
        private CodeUnit(string projectDefaultNamespace,
            IReadOnlyList<UsingDirectiveInfo> directives, IReadOnlyList<NamespaceInfo> namespaces,
            IReadOnlyList<TypeInfo> types, IReadOnlyList<ExternAliasInfo> externAliases)

            : base(projectDefaultNamespace, directives, namespaces, types, externAliases)
        { }

        public static CodeUnit FromFile(string fileName, string projectDefaultNamespace)
        {
            return FromStream(new AntlrInputStream(File.OpenRead(fileName)), projectDefaultNamespace);
        }

        public static CodeUnit FromText(string text, string projectDefaultNamespace)
        {
            return FromStream(new AntlrInputStream(text), projectDefaultNamespace);
        }

        private static CodeUnit FromStream(ICharStream stream, string projectDefaultNamespace)
        {
            var lexer = new CSharpLexer(stream);
            var parser = new CSharpParser(new CommonTokenStream(lexer));

            var listener = new ThrowExceptionListener();
            parser.AddErrorListener(listener);

            var visitor = new NamespaceVisitor();
            var ns = visitor.VisitCompilation_unit(parser.compilation_unit())
                ?? throw new InvalidOperationException("something went wrong during parsing");

            return new CodeUnit(projectDefaultNamespace, 
                ns.UsingDirectives, ns.Namespaces, ns.Types, ns.ExternAliases);
        }
    }
}
