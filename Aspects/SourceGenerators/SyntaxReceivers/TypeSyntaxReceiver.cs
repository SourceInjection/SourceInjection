using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace Aspects.SourceGenerators.SyntaxReceivers
{
    internal class TypeSyntaxReceiver : ISyntaxContextReceiver
    {
        private readonly Predicate<Common.TypeInfo> _predicate;
        private readonly List<Common.TypeInfo> _identifiedContexts = new List<Common.TypeInfo>(256);

        public TypeSyntaxReceiver(Predicate<Common.TypeInfo> predicate)
        {
            _predicate = predicate;
        }

        public IReadOnlyList<Common.TypeInfo> IdentifiedTypes => _identifiedContexts;

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if(context.Node is TypeDeclarationSyntax node && context.SemanticModel.GetDeclaredSymbol(node) is INamedTypeSymbol symbol)
            {
                var typeInfo = new Data.TypeInfo(node, symbol);
                if(_predicate(typeInfo))
                    _identifiedContexts.Add(typeInfo);
            }
        }
    }
}
