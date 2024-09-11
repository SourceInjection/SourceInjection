using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using TypeInfo = Aspects.SourceGeneration.Common.TypeInfo;

namespace Aspects.SourceGeneration.SyntaxReceivers
{
    internal class TypeSyntaxReceiver : ISyntaxContextReceiver
    {
        private readonly Predicate<TypeInfo> _predicate;
        private readonly List<TypeInfo> _identifiedTypes = new List<TypeInfo>(256);

        public TypeSyntaxReceiver(Predicate<TypeInfo> predicate)
        {
            _predicate = predicate;
        }

        public IReadOnlyList<TypeInfo> IdentifiedTypes => _identifiedTypes;

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is TypeDeclarationSyntax node && context.SemanticModel.GetDeclaredSymbol(node) is INamedTypeSymbol symbol)
            {
                var type = new TypeInfo(context, node, symbol);
                TypeInfo.Add(type.Symbol.ToDisplayString(), type);

                if (_predicate(type))
                    _identifiedTypes.Add(type);
            }
        }
    }
}
