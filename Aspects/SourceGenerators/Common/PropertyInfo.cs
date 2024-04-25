﻿

using Aspects.LoadingBehaviors;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Aspects.SourceGenerators.Common
{
    internal class PropertyInfo
    {
        private readonly Lazy<bool> _isDataMember;
        private readonly Lazy<IFieldSymbol> _linkedField;
        private readonly Lazy<bool> _hidesBasePropertyByName;

        public PropertyInfo(PropertyDeclarationSyntax syntaxNode, IPropertySymbol symbol)
        {
            Symbol = symbol;
            SyntaxNode = syntaxNode;

            _isDataMember = new Lazy<bool>(
                () => Property.IsDataMember(syntaxNode));

            _linkedField = new Lazy<IFieldSymbol>(LoadField);

            _hidesBasePropertyByName = new Lazy<bool>(
                () => Symbol.HidesBasePropertyByName());
        }

        public IPropertySymbol Symbol { get; }

        public PropertyDeclarationSyntax SyntaxNode { get; }

        public IFieldSymbol LinkedField => _linkedField.Value;

        public bool IsDataMember => _isDataMember.Value;

        public bool HidesBasePropertyByName => _hidesBasePropertyByName.Value;

        private IFieldSymbol LoadField()
        {
            if (!Property.TryGetReturnedIdentifier(SyntaxNode, out var identifier))
                return null;

            return Symbol.ContainingType?.LocalVisibleFields()
                .FirstOrDefault(f => f.Name == identifier.ValueText);
        }
    }
}
