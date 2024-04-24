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
        private readonly Predicate<TypeInfo> _predicate;
        private readonly List<TypeInfo> _identifiedContexts = new List<TypeInfo>(256);

        public TypeSyntaxReceiver(Predicate<TypeInfo> predicate)
        {
            _predicate = predicate;
        }

        public IReadOnlyList<TypeInfo> IdentifiedTypes => _identifiedContexts;

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is TypeDeclarationSyntax node && context.SemanticModel.GetDeclaredSymbol(node) is INamedTypeSymbol symbol)
            {
                var typeInfo = new TypeInfo(node, symbol);
                if (_predicate(typeInfo))
                    _identifiedContexts.Add(typeInfo);
            }
        }
    }
}
