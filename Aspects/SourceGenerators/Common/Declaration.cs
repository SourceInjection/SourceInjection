using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Aspects.SourceGenerators.Common
{
    internal static class Declaration
    {
        public static string GetName(TypeDeclarationSyntax node)
        {
            if (node.TypeParameterList is null)
                return node.Identifier.Text;

            var typeDec = GetText(node);
            return typeDec.Substring(typeDec.IndexOf(node.Identifier.Text)).Trim();
        }

        public static string GetText(TypeDeclarationSyntax node)
        {
            var nodeText = node.GetText().ToString();
            nodeText = nodeText.Substring(0, nodeText.IndexOf('{'));
            nodeText = RemoveAttributes(nodeText).Trim();

            if (nodeText.Contains('>'))
                return nodeText.Substring(0, nodeText.IndexOf('>') + 1);
            return nodeText.Substring(0, nodeText.IndexOf(node.Identifier.Text) + node.Identifier.Text.Length);
        }

        public static IEnumerable<INamedTypeSymbol> InheritanceFromBottomToTop(INamedTypeSymbol symbol)
        {
            var stack = new Stack<INamedTypeSymbol>();

            while (symbol != null)
            {
                stack.Push(symbol);
                symbol = symbol.BaseType;
            }

            while (stack.Count > 0)
                yield return stack.Pop();
        }

        public static bool HasPartialModifier(TypeDeclarationSyntax typeDeclaration)
        {
            return typeDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
        }

        private static string RemoveAttributes(string text)
        {
            while (text.Contains('['))
            {
                (var idx, var length) = AttributeLocation(text);
                text = text.Remove(idx, length);
            }
            return text;
        }

        private static (int Idx, int Length) AttributeLocation(string text)
        {
            var idx = text.IndexOf('[');
            int length = 1;
            int nestLevel = 0;

            while (idx + length < text.Length && (nestLevel > 0 || text[idx + length] != ']'))
            {
                if (text[idx + length] == '[')
                    nestLevel++;
                else if (text[idx + length] == ']')
                    nestLevel--;
                length++;
            }
            if (idx + length < text.Length)
                length++;

            return (idx, length);
        }
    }
}
