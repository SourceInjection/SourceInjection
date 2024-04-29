using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SyntaxReceivers
{
    /// <summary>
    /// A <see cref="ISyntaxContextReceiver"/> that matches types using a defined <see cref="Predicate{T}"/>.
    /// </summary>
    internal class TypeSyntaxReceiver : ISyntaxContextReceiver
    {
        private readonly Predicate<TypeInfo> _predicate;
        private readonly List<TypeInfo> _identifiedTypes = new List<TypeInfo>(256);

        /// <summary>
        /// Creates an instance of this type matching <see cref="ISyntaxContextReceiver"/>.
        /// </summary>
        /// <param name="predicate">the <see cref="Predicate{T}"/> which must be fullfilled on <see cref="TypeInfo"/>s 
        /// so that it is included in <see cref="IdentifiedTypes"/>.</param>
        public TypeSyntaxReceiver(Predicate<TypeInfo> predicate)
        {
            _predicate = predicate;
        }

        /// <summary>
        /// Contains the <see cref="TypeInfo"/> which fullfilled the given <see cref="Predicate{T}"/>.
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
