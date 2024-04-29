using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SyntaxReceivers
{
    /// <summary>
    /// A syntax receiver that matches types using a defined predicate
    /// </summary>
    internal class TypeSyntaxReceiver : ISyntaxContextReceiver
    {
        private readonly Predicate<TypeInfo> _predicate;
        private readonly List<TypeInfo> _identifiedTypes = new List<TypeInfo>(256);

        /// <summary>
        /// Creates an instance of this type matching syntax receiver
        /// </summary>
        /// <param name="predicate">the predicate which must be fullfilled</param>
        public TypeSyntaxReceiver(Predicate<TypeInfo> predicate)
        {
            _predicate = predicate;
        }

        /// <summary>
        /// Contains the types which fullfilled the given predicate
        /// </summary>
        public IReadOnlyList<TypeInfo> IdentifiedTypes => _identifiedTypes;

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is TypeDeclarationSyntax node && context.SemanticModel.GetDeclaredSymbol(node) is INamedTypeSymbol symbol)
            {
                var typeInfo = new TypeInfo(context, node, symbol);
                if (_predicate(typeInfo))
                    _identifiedTypes.Add(typeInfo);
            }
        }
    }
}
