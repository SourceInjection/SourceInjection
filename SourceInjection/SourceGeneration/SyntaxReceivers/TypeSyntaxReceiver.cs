using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using TypeInfo = SourceInjection.SourceGeneration.Common.TypeInfo;

namespace SourceInjection.SourceGeneration.SyntaxReceivers
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
                var type = new TypeInfo(context, node, symbol);
                TypeInfo.Consider(symbol.ToDisplayString(), type);

                if(_predicate(symbol))
                    _identifiedTypes.Add(type);
            }
        }
    }
}
