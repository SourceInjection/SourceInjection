using Aspects.LoadingBehaviors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Aspects.SourceGenerators.Common
{
    internal class TypeInfo
    {
        private readonly Lazy<bool> _hasPartialModifier;
        private readonly Lazy<string> _declaration;
        private readonly Lazy<string> _name;
        private readonly Lazy<List<INamedTypeSymbol>> _orderedInheritance;
        private readonly Lazy<List<PropertyInfo>> _localProperties;

        public TypeInfo(GeneratorSyntaxContext context, TypeDeclarationSyntax syntaxNode, INamedTypeSymbol typeSymbol)
        {
            SyntaxNode = syntaxNode;
            Symbol = typeSymbol;

            _hasPartialModifier = new Lazy<bool>(
                () => Common.Declaration.HasPartialModifier(syntaxNode));

            _declaration = new Lazy<string>(
                () => Common.Declaration.GetText(syntaxNode));

            _name = new Lazy<string>(
                () => Common.Declaration.GetName(syntaxNode));

            _orderedInheritance = new Lazy<List<INamedTypeSymbol>>(
                () => Common.Declaration.InheritanceFromBottomToTop(typeSymbol).ToList());

            _localProperties = new Lazy<List<PropertyInfo>>(
                () => LoadProperties(context));
        }

        public TypeDeclarationSyntax SyntaxNode { get; }

        public INamedTypeSymbol Symbol { get; }

        public bool HasPartialModifier => _hasPartialModifier.Value;

        public string Declaration => _declaration.Value;

        public string Name => _name.Value;

        public IEnumerable<PropertyInfo> LocalProperties => _localProperties.Value;

        public IEnumerable<INamedTypeSymbol> InheritanceFromBottomToTop(bool includeSelf = false)
        {
            if (includeSelf)
                return _orderedInheritance.Value;
            return _orderedInheritance.Value.Take(_orderedInheritance.Value.Count - 1);
        }

        public IEnumerable<ISymbol> Members(bool includeInherited = false)
        {
            if (!includeInherited)
                return Symbol.GetMembers();

            return InheritanceFromBottomToTop(true).SelectMany(cl => cl.GetMembers());
        }

        private List<PropertyInfo> LoadProperties(GeneratorSyntaxContext context)
        {
            var localProperties = new HashSet<IPropertySymbol>(
                Symbol.GetMembers().OfType<IPropertySymbol>(), SymbolEqualityComparer.Default);

            return SyntaxNode.DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .Select(pds => new { PropertyDeclarationSyntax = pds, Symbol = context.SemanticModel.GetDeclaredSymbol(pds) })
                .Where(tuple => tuple.Symbol is IPropertySymbol && localProperties.Contains(tuple.Symbol, SymbolEqualityComparer.Default))
                .Select(tuple => new PropertyInfo(tuple.PropertyDeclarationSyntax, (IPropertySymbol)tuple.Symbol))
                .ToList();
        }
    }
}
