using Aspects.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SyntaxReceivers
{
    internal class TypeSyntaxReceiver : ISyntaxContextReceiver
    {
        private readonly Predicate<INamedTypeSymbol> _predicate;
        private readonly List<TypeInfo> _identifiedTypes = new List<TypeInfo>(256);

        public TypeSyntaxReceiver(Predicate<INamedTypeSymbol> predicate)
        {
            _predicate = predicate;
        }

        public IReadOnlyList<TypeInfo> IdentifiedTypes => _identifiedTypes;

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is TypeDeclarationSyntax node && context.SemanticModel.GetDeclaredSymbol(node) is INamedTypeSymbol symbol)
            {
                var name = symbol.ToDisplayString();
                if (Types.Get(name) == null)
                    Types.Add(name, symbol);

                if (_predicate(symbol))
                    _identifiedTypes.Add(new TypeInfo(context, node, symbol));
            }
        }
    }
}
