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

        public TypeInfo(TypeDeclarationSyntax syntaxNode, INamedTypeSymbol typeSymbol)
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

            lköadshlf

        }

        public TypeDeclarationSyntax SyntaxNode { get; }

        public INamedTypeSymbol Symbol { get; }

        public bool HasPartialModifier => _hasPartialModifier.Value;

        public string Declaration => _declaration.Value;

        public string Name => _name.Value;

        public IEnumerable<PropertyInfo> LocalProperties => _localProperties.Value;

        public IEnumerable<INamedTypeSymbol> OrderedInheritance(bool includeSelf = false)
        {
            if (includeSelf)
                return _orderedInheritance.Value;
            return _orderedInheritance.Value.Take(_orderedInheritance.Value.Count - 1);
        }

        public IEnumerable<ISymbol> Members(bool includeInherited = false)
        {
            if (!includeInherited)
                return Symbol.GetMembers();

            return OrderedInheritance(true).SelectMany(cl => cl.GetMembers());
        }
    }
}
